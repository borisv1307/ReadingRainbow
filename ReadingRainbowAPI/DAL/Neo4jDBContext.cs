using Neo4j.Driver;

namespace ReadingRainbowAPI.DAL
{
    public class Neo4jDBContext : INeo4jDBContext
    {
        private IDriver _db { get; set; }
        // private IDriver _neoClient { get; set; }
        // public IClientSessionHandle Session { get; set; }
 
        public Neo4jDBContext()
        {

            // IOptions<Settings> configuration
            //var _neoUserName = configuration.Value.Connection;
            //var _neoPassword = configuration.Value;
 
            //_db = _neoClient.GetDatabase(configuration.Value.DatabaseName);
            _db = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("neo4j", "neo"));
        }

        // Add disposing
      
    }
}