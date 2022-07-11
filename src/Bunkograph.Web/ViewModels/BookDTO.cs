namespace Bunkograph.Web.ViewModels
{
    public class BookDTO
    {
        public int BookId { get; set; }
        public Dictionary<string, BookEditionDTO> Editions { get; set; } = new Dictionary<string, BookEditionDTO>();
    }
}
