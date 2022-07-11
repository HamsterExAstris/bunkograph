using System.ComponentModel.DataAnnotations;

namespace Bunkograph.Models
{
    public class Language
    {
        [MaxLength(2)]
        public string LanguageId { get; set; } = string.Empty;
    }
}
