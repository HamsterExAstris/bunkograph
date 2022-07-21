using Bunkograph.DAL;
using Bunkograph.Models;
using Bunkograph.Web.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bunkograph.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        private readonly BunkographContext _context;

        public SeriesController(BunkographContext context)
        {
            _context = context;
        }

        // GET: api/<SeriesController>
        [HttpGet]
        public IAsyncEnumerable<SeriesDTO> Get()
        {
            return _context.Series
                .Include(s => s.SeriesLicenses)
                .OrderBy(s => s.EnglishName)
                .Select(s => new SeriesDTO
                {
                    SeriesId = s.SeriesId,
                    EnglishName = s.EnglishName,
                    OriginalName = s.OriginalName,
                    Licenses = s.SeriesLicenses.Select(sl => new SeriesLicenseDTO
                    {
                        SeriesLicenseId = sl.SeriesLicenseId,
                        Language = sl.Language,
                        Publisher = sl.Publisher,
                        CompletionStatus = sl.CompletionStatus
                    })
                })
                .AsAsyncEnumerable();
        }

        // GET api/<SeriesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Series? series = await _context.Series.FindAsync(id);
            return series is null
                ? NotFound()
                : Ok(series);
        }

        // GET api/<SeriesController>/volumes/5
        [HttpGet("volumes/{id}")]
        public async Task<IActionResult> GetVolumes(int id)
        {
            Series? series = await _context.Series
                .Include(s => s.SeriesBooks)
                .ThenInclude(sb => sb.Book)
                .ThenInclude(b => b.Editions)
                .ThenInclude(be => be.SeriesLicense)
                .FirstOrDefaultAsync(s => s.SeriesId == id);
            if (series is null)
            {
                return NotFound();
            }

            SeriesDTO? result = new SeriesDTO
            {
                SeriesId = series.SeriesId
            };

            IEnumerable<BookDTO>? books = series.SeriesBooks.Select(s => s.Book)
                .Select(b => new BookDTO
                {
                    BookId = b.BookId,
                    Editions = b.Editions.ToDictionary(k => k.SeriesLicense.LanguageId, v => new BookEditionDTO
                    {
                        ReleaseDate = v.ReleaseDate.ToDateTime(TimeOnly.MinValue),
                        Language = v.SeriesLicense.LanguageId,
                        SeriesLicenseId = v.SeriesLicenseId
                    })
                });
            result.Books = books;
            return Ok(result);
        }

        // GET api/<SeriesController>/volumes/5
        [HttpPost("volumes/{id}")]
        [Authorize("Luxae.ChargingMabab")]
        public async Task<IActionResult> PostVolumes(int id, SeriesDTO seriesDto)
        {
            Series? series = await _context.Series
                .Include(s => s.SeriesBooks)
                .ThenInclude(sb => sb.Book)
                .ThenInclude(b => b.Editions)
                .ThenInclude(be => be.SeriesLicense)
                .FirstOrDefaultAsync(s => s.SeriesId == id);
            if (series is null)
            {
                return NotFound();
            }

            foreach (BookDTO? bookDto in seriesDto.Books)
            {
                SeriesBook? seriesBook = series.SeriesBooks.FirstOrDefault(sb => sb.BookId == bookDto.BookId);
                if (seriesBook is null)
                {
                    Book? book = await _context.Books.FindAsync(bookDto.BookId);
                    if (book is null)
                    {
                        return BadRequest("Book " + bookDto.BookId + " not found.");
                    }

                    decimal sortOrder = series.SeriesBooks.Any()
                        ? series.SeriesBooks.Select(s => s.SortOrder).Max() + 1
                        : 1;
                    seriesBook = new SeriesBook
                    {
                        Series = series,
                        Book = book,
                        SortOrder = sortOrder
                    };
                    series.SeriesBooks.Add(seriesBook);
                }

                // No way to delete here. This is intentional.
                foreach (KeyValuePair<string, BookEditionDTO> bookEditionDto in bookDto.Editions)
                {
                    BookEdition? bookEdition = seriesBook.Book.Editions.FirstOrDefault(be => be.SeriesLicense.LanguageId == bookEditionDto.Key);
                    if (bookEdition is null)
                    {
                        // The series license has to exist already for use to use this page.
                        SeriesLicense? license = await _context.SeriesLicenses.FindAsync(bookEditionDto.Key);
                        if (license is null)
                        {
                            return BadRequest("Series license " + bookEditionDto.Key + " not found.");
                        }

                        bookEdition = new BookEdition(license, DateOnly.FromDateTime(bookEditionDto.Value.ReleaseDate));
                        seriesBook.Book.Editions.Add(bookEdition);
                    }
                    bookEdition.ReleaseDate = DateOnly.FromDateTime(bookEditionDto.Value.ReleaseDate);
                }
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        // POST api/<SeriesController>
        [HttpPost]
        public async Task Post([FromBody] Series value)
        {
            _context.Series.Add(value);
            await _context.SaveChangesAsync();
        }

        // PUT api/<SeriesController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Series value)
        {
            Series? series = await _context.Series.FindAsync(id);
            if (series is not null)
            {
                _context.Entry(series).CurrentValues.SetValues(value);
            }
            else
            {
                _context.Series.Add(value);
            }
            await _context.SaveChangesAsync();
        }

        // DELETE api/<SeriesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            Series? series = await _context.Series.FindAsync(id);
            if (series is not null)
            {
                _context.Series.Remove(series);
                await _context.SaveChangesAsync();
            }
        }
    }
}
