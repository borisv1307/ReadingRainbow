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

        public string Email {get; set;}

        public string HashedPassword {get; set;}

        public string EmailConfirmed {get; set;}

        public string Token {get; set;}
    }

}