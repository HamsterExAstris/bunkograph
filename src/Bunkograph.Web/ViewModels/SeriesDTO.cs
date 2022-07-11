namespace Bunkograph.Web.ViewModels
{
    public class SeriesDTO
    {
        public int SeriesId { get; set; }
        public IEnumerable<BookDTO> Books { get; set; } = Enumerable.Empty<BookDTO>();
    }
}
