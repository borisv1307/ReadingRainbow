using System.Collections.Generic;

namespace ReadingRainbowAPI.Models
{
    public class PopularityResult : Neo4jEntity
    {
        public PopularityResult()
        {
            Label = "PopularityResult";
        }
        public int Popularity { get; set; }
        public Book Book { get; set; }
    }
}