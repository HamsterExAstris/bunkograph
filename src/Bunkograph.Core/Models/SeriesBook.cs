using Microsoft.EntityFrameworkCore;

namespace Bunkograph.Models
{
    public class SeriesBook
    {
        private Series? _series;
        private Book? _book;

        public int SeriesId { get; set; }
        public Series Series
        {
            get => _series ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Series));
            set => _series = value;
        }

        public int BookId { get; set; }
        public Book Book
        {
            get => _book ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Book));
            set => _book = value;
        }

        [Precision(6, 2)]
        public decimal SortOrder { get; set; }
        public string SortOrderString => (SortOrder == (int)SortOrder)
                    ? SortOrder.ToString("F0")
                    : SortOrder.ToString("F2");
        public string? DisplayIndex { get; set; }
    }
}
