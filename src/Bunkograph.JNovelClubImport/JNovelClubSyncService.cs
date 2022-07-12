
using Bunkograph.DAL;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bunkograph.JNovelClubImport
{
    internal class JNovelClubSyncService : IHostedService
    {
        private readonly JNovelClubClient _client;
        private readonly IServiceScopeFactory _scopeFactory;


        public JNovelClubSyncService(IServiceScopeFactory scopeFactory, JNovelClubClient client)
        {
            _client = client;
            _scopeFactory = scopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // await _client.LoginAsync();
            await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
            await using BunkographContext? context = scope.ServiceProvider.GetRequiredService<BunkographContext>();

            Models.Publisher? publisher = await context.Publishers.FirstOrDefaultAsync(p => p.Name == "J-Novel Club");
            if (publisher is null)
            {
                publisher = new Models.Publisher("J-Novel Club");
                context.Publishers.Add(publisher);
            }

            IEnumerable<DTOs.Series>? allSeries = await _client.GetSeriesAsync();
            foreach (DTOs.Series? apiSeries in allSeries.Where(s => s.Type == "NOVEL"))
            {
                Models.SeriesLicense? seriesLicense = null;
                Models.Series? dbSeries = await context.Series
                    .Include(s => s.SeriesBooks)
                    .ThenInclude(sb => sb.Book)
                    .ThenInclude(b => b.Editions)
                    .ThenInclude(be => be.SeriesLicense)
                    .FirstOrDefaultAsync(s => s.EnglishKey == apiSeries.Slug);
                if (dbSeries is null)
                {
                    dbSeries = new Models.Series(apiSeries.OriginalTitle ?? string.Empty, apiSeries.Title ?? string.Empty)
                    {
                        EnglishKey = apiSeries.Slug
                    };
                    context.Series.Add(dbSeries);
                }
                else
                {
                    seriesLicense = await context.SeriesLicenses.FirstOrDefaultAsync(sl => sl.LanguageId == "en"
                        && sl.PublisherId == publisher.PublisherId
                        && sl.SeriesId == dbSeries.SeriesId);
                }

                if (seriesLicense is null)
                {
                    seriesLicense = new Models.SeriesLicense
                    {
                        Publisher = publisher,
                        LanguageId = "en",
                        Series = dbSeries
                    };
                    context.SeriesLicenses.Add(seriesLicense);
                }

                IEnumerable<DTOs.Volume>? volumes = await _client.GetSeriesVolumesAsync(apiSeries);
                foreach (DTOs.Volume? apiVolume in volumes)
                {
                    Models.SeriesBook? dbSeriesBook = dbSeries.SeriesBooks.FirstOrDefault(sb => sb.Book.EnglishKey == apiVolume.Slug);
                    if (dbSeriesBook is null)
                    {
                        DTOs.Creator? author = apiVolume.Creators.FirstOrDefault(c => c.Role == "AUTHOR");

                        dbSeriesBook = new Models.SeriesBook
                        {
                            Book = new Models.Book()
                            {
                                EnglishKey = apiVolume.Slug,
                                Title = apiVolume.Title
                            },
                            SortOrder = apiVolume.Number
                        };

                        if (author is not null)
                        {
                            Models.Author? modelAuthor = await context.Authors.FirstOrDefaultServerAndLocalAsync(a => a.RomanizedName == author.Name);
                            if (modelAuthor is null)
                            {
                                modelAuthor = new Models.Author()
                                {
                                    JapaneseName = author.OriginalName,
                                    RomanizedName = author.Name
                                };
                                context.Authors.Add(modelAuthor);
                            }
                            if (modelAuthor.JapaneseName == "Default Original Author" || modelAuthor.JapaneseName == string.Empty)
                            {
                                modelAuthor.JapaneseName = null;
                            }
                            dbSeriesBook.Book.Author = modelAuthor;
                        }

                        dbSeries.SeriesBooks.Add(dbSeriesBook);
                    }

                    DateOnly publishing = apiVolume.Publishing.HasValue ? DateOnly.FromDateTime(apiVolume.Publishing.Value) : DateOnly.MaxValue;
                    Models.BookEdition? edition = dbSeriesBook.Book.Editions.FirstOrDefault(e => e.SeriesLicense.LanguageId == "en");
                    if (edition is null)
                    {
                        edition = new Models.BookEdition(seriesLicense, publishing)
                        {
                            Publisher = publisher
                        };
                        dbSeriesBook.Book.Editions.Add(edition);
                    }
                    else
                    {
                        // Update the date. This is the only field we change.
                        if (edition.ReleaseDate != publishing)
                        {
                            edition.ReleaseDate = publishing;
                        }
                    }
                }
            }

            await context.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
