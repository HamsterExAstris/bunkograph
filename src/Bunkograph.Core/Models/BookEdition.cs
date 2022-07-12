namespace Bunkograph.Models
{
    public class BookEdition
    {
        public int BookEditionId { get; set; }

        private Book? _book;
        public Book Book
        {
            get => _book ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Book));
            set => _book = value;
        }

        public int PublisherId { get; set; }
        private Publisher? _publisher;
        public Publisher Publisher
        {
            get => _publisher ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Publisher));
            set => _publisher = value;
        }

        public int SeriesLicenseId { get; set; }
        private SeriesLicense? _seriesLicense;
        public SeriesLicense SeriesLicense
        {
            get => _seriesLicense ?? throw new InvalidOperationException("Uninitialized property: " + nameof(SeriesLicense));
            set => _seriesLicense = value;
        }

        public DateOnly ReleaseDate { get; set; }

        public BookEdition(SeriesLicense seriesLicense, DateOnly releaseDate)
        {
            SeriesLicense = seriesLicense;
            ReleaseDate = releaseDate;
        }

        public BookEdition(int seriesLicenseId, DateOnly releaseDate)
        {
            SeriesLicenseId = seriesLicenseId;
            ReleaseDate = releaseDate;
        }
    }
}
