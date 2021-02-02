using System.Text.Json.Serialization;

namespace ReadingRainbowAPI.Relationships
{
    public class Neo4jRelationship
    {
        [JsonIgnore]
        public string Name {get; set;}

        public Neo4jRelationship()
        {

        }
    }
}