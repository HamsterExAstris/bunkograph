namespace Bunkograph.Models
{
    public class SeriesLicense
    {
        private Series? _series;
        private Language? _language;
        private Publisher? _publisher;

        public int SeriesLicenseId { get; set; }

        public int SeriesId { get; set; }
        public Series Series
        {
            get => _series ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Series));
            set => _series = value;
        }

        public string LanguageId { get; set; } = string.Empty;
        public Language Language
        {
            get => _language ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Language));
            set => _language = value;
        }

        public int PublisherId { get; set; }
        public Publisher Publisher
        {
            get => _publisher ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Publisher));
            set => _publisher = value;
        }

        public CompletionStatus? CompletionStatus { get; set; }
    }
}
