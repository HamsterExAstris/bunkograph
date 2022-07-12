using Bunkograph.Models;

namespace Bunkograph.Web.ViewModels
{
    public class Volume
    {
        public string ReleaseDate { get; set; }

        public decimal? VolumeNumber { get; set; }

        public string Label { get; set; }

        public string Language { get; set; }

        public Volume(SeriesBook seriesBook, BookEdition bookEdition)
        {
            ReleaseDate = bookEdition.ReleaseDate.ToString("o");
            VolumeNumber = (seriesBook.SortOrder == (int)seriesBook.SortOrder)
                ? Math.Round(seriesBook.SortOrder, 0)
                : Math.Round(seriesBook.SortOrder, 2);
            Label = seriesBook.DisplayIndex ?? seriesBook.SortOrderString;
            Language = bookEdition.SeriesLicense.LanguageId;
        }
    }
}
