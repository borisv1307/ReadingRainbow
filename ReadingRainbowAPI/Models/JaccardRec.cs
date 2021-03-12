using System.Collections.Generic;

namespace ReadingRainbowAPI.Models
{
    public class JaccardRec : Neo4jEntity
    {
        public JaccardRec()
        {
            Label = "JaccardRec";
        }
        public double JaccardIndex { get; set; }
        public BookList BookList { get; set; }
    }
}