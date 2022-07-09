namespace Bunkograph.JNovelClubImport.DTOs
{
    internal class Volume
    {
        public string? Title { get; set; }
        public string? OriginalPublisher { get; set; }
        /// <summary>
        /// Japanese name of the specific imprint. *Not* a translation of <see cref="OriginalPublisher"/>.
        /// </summary>
        public string? Label { get; set; }
        public string? Slug { get; set; }
        public int Number { get; set; }
        public DateTime? Publishing { get; set; }
        public IEnumerable<Creator> Creators { get; set; } = Enumerable.Empty<Creator>();
    }
}
