using System;
namespace springmusicdotnetcore.Models
{
    public class Album
    {
        public long Id { get; set; }
        public String title { get; set; }
        public String artist { get; set; }
        public Int16 releaseYear { get; set; }
        public String genre { get; set; }
    }
}
