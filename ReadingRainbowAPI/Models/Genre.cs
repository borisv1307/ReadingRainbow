namespace ReadingRainbowAPI.Models
{
    public class Genre : Neo4jEntity
    {
        public Genre()
        {
            Label = "Genre";
        }

        public string Name {get; set;}
        public string Description{ get; set;}
    }
}