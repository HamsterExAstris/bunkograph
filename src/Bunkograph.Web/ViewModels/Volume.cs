using Bunkograph.Models;

namespace Bunkograph.Web.ViewModels
{
    public class Volume
    {
        public string Release { get; set; }

        public decimal? VolumeNumber { get; set; }

        public string Label { get; set; }

        public Volume(BookEdition bookEdition)
        {
            SeriesBook? seriesBook = bookEdition.Book.SeriesBooks.FirstOrDefault();
            if (seriesBook is null)
            {
                throw new ArgumentException("Book is not part of a series.", nameof(bookEdition));
            }

            Release = bookEdition.ReleaseDate.ToString("o");
            VolumeNumber = (seriesBook.SortOrder == (int)seriesBook.SortOrder)
                ? Math.Round(seriesBook.SortOrder, 0)
                : Math.Round(seriesBook.SortOrder, 2);
            Label = seriesBook.DisplayIndex ?? seriesBook.SortOrderString;
        }
    }
}
