using Bunkograph.Models;

namespace Bunkograph.Web.ViewModels
{
    public class Volume
    {
        /// <summary>
        /// Release date in Unix timestamp.
        /// </summary>
        public long Release { get; set; }

        public decimal? VolumeNumber { get; set; }

        public string Label { get; set; }

        public Volume(BookEdition bookEdition)
        {
            SeriesBook? seriesBook = bookEdition.Book.SeriesBooks.FirstOrDefault();
            if (seriesBook is null)
            {
                throw new ArgumentException("Book is not part of a series.", nameof(bookEdition));
            }

            DateTimeOffset dateTime = new DateTimeOffset(bookEdition.ReleaseDate.ToDateTime(TimeOnly.MinValue));
            Release = dateTime.ToUnixTimeSeconds();

            VolumeNumber = seriesBook.SortOrder;
            Label = seriesBook.DisplayIndex ?? seriesBook.SortOrderString;
        }
    }
}
