using System.ComponentModel.DataAnnotations;

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

        private Publisher? _publisher;
        public Publisher Publisher
        {
            get => _publisher ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Publisher));
            set => _publisher = value;
        }

        [MaxLength(2)]
        public string Language { get; set; }
        public DateOnly ReleaseDate { get; set; }

        public BookEdition(string language, DateOnly releaseDate)
        {
            Language = language;
            ReleaseDate = releaseDate;
        }
    }
}
