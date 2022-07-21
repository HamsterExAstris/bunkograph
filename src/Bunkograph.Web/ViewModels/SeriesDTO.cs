namespace Bunkograph.Web.ViewModels
{
    public class SeriesDTO
    {
        public int SeriesId { get; set; }
        public string? EnglishName { get; set; }
        public string? OriginalName { get; set; }
        public IEnumerable<BookDTO> Books { get; set; } = Enumerable.Empty<BookDTO>();
        public IEnumerable<SeriesLicenseDTO> Licenses { get; set; } = Enumerable.Empty<SeriesLicenseDTO>();
    }
}
