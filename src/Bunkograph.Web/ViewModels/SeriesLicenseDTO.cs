using Bunkograph.Models;

namespace Bunkograph.Web.ViewModels
{
    public class SeriesLicenseDTO
    {
        public int? SeriesLicenseId { get; set; }
        public Publisher? Publisher { get; set; }
        public Language? Language { get; set; }
        public CompletionStatus? CompletionStatus { get; set; }
    }
}
