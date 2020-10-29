using Neo4j.Driver;
using Neo4jClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Text;
using System;

namespace ReadingRainbowAPI.DAL
{
    public class Neo4jDBContext : INeo4jDBContext
    {
        public IGraphClient dbClient {get; private set;}
        private IAsyncSession _dbSession {get; set;}
 
        public Neo4jDBContext()
        {
            // Pass in configuration options
            // set up client with configuration options
 
            //_db = _neoClient.GetDatabase(configuration.Value.DatabaseName);
            dbClient = new GraphClient(new Uri("bolt://localhost:7687"), "Neo4j", "Neo4jPassword");
            dbClient.Connect();
        }
}
      
}