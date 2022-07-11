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

        public string LanguageId { get; set; }
        private Language? _language;
        public Language Language
        {
            get => _language ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Language));
            set => _language = value;
        }

        public DateOnly ReleaseDate { get; set; }

        public BookEdition(string languageId, DateOnly releaseDate)
        {
            LanguageId = languageId;
            ReleaseDate = releaseDate;
        }
    }
}
