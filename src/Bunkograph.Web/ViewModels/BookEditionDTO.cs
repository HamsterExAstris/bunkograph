namespace Bunkograph.Web.ViewModels
{
    public class BookEditionDTO
    {
        public int PublisherId { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Language { get; set; }
        public int SeriesLicenseId { get; set; }
    }
}
