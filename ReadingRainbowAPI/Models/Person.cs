namespace ReadingRainbowAPI.Models
{
    public class Person : Neo4jEntity
    {
        public string Name {get; set;}
        public string Profile {get; set;}
        public string Portrait {get; set;}
    }
}