using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using System;

namespace ReadingRainbowAPI.Models
{
    public class Neo4jEntity : IEntity
    {

        protected Neo4jEntity()
        {
            LastUpdated = DateTime.UtcNow;

        }
        [JsonIgnore]
        [XmlIgnore]
        public string Label {get; protected set;}
        
        [JsonIgnore]
        [XmlIgnore]
        public DateTimeOffset LastUpdated {get; set;}
    }
}