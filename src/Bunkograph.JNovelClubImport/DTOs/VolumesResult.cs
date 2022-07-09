namespace Bunkograph.JNovelClubImport.DTOs
{
    internal class VolumesResult : PaginatedResultBase
    {
        public IEnumerable<Volume> Volumes { get; set; } = Enumerable.Empty<Volume>();
    }
}
