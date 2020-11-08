namespace ReadingRainbowAPI.Models
{
    public class Person : Neo4jEntity
    {
        public Person()
        {
            Label = "Person";
        }

        public string Name {get; set;}
        public string Profile {get; set;}
        public string Portrait {get; set;}

        public string HashedPassword {get; set;}
    }
}