namespace Bunkograph.JNovelClubImport.DTOs
{
    internal class SeriesResult : PaginatedResultBase
    {
        public IEnumerable<Series> Series { get; set; } = Enumerable.Empty<Series>();
    }
}
