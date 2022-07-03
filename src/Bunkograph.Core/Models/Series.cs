﻿namespace Bunkograph.Models
{
    public class Series
    {
        public int SeriesId { get; set; }
        public string OriginalName { get; set; }
        public string EnglishName { get; set; }

        public Series(string originalName, string englishName)
        {
            OriginalName = originalName;
            EnglishName = englishName;
        }
    }
}
