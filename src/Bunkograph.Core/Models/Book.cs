using System.Text;

namespace Bunkograph.Models
{
    public class Book
    {
        public int BookId { get; set; }

        private Author? _author;
        public Author Author
        {
            get => _author ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Author));
            set => _author = value;
        }

        public string? Title { get; set; }
        public string DisplayName
        {
            get
            {
                StringBuilder? result = new StringBuilder();

                foreach (SeriesBook? series in SeriesBooks)
                {
                    if (result.Length > 0)
                    {
                        result.Append(", ");
                    }
                    result.Append(series.Series.EnglishName);
                    result.Append(" #");
                    result.Append(series.DisplayIndex ?? series.SortOrder.ToString());
                }

                if (Title is not null)
                {
                    if (result.Length > 0)
                    {
                        result.Append(": ");
                    }
                    result.Append(Title);
                }

                return result.ToString();
            }
        }

        public ICollection<SeriesBook> SeriesBooks { get; set; } = new List<SeriesBook>();
        public ICollection<BookEdition> Editions { get; set; } = new List<BookEdition>();
    }
}
