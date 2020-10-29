using System.Collections.Generic;

namespace ReadingRainbowAPI.Models
{
    public class Book : Neo4jEntity
    {
        public string Id {get; set;}
        public string BookInformationLink {get; set;}
        public string Title {get; set;}
        public List<string> Authors = new List<string>();
        public string Thumbnail {get; set;}
        public string SmallThumbnail {get; set;}
        public string PublishDate {get; set;}
        public string NumberPages {get; set;}
        public string Description {get; set;}
        public string ISBN_10 {get; set;}      
        public string ISBN_13 {get; set;}   
        public List<string> Cateogries = new List<string>();

    }   
}