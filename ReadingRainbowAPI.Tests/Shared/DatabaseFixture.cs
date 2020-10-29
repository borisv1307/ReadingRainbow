using Neo4j.Driver;
using Neo4jClient;
using System;
using Xunit;
using Microsoft.Extensions.Configuration;

public class DatabaseFixture : IDisposable
{

    public DatabaseFixture()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        var neoUserName = config.GetSection("appSettings").GetValue<string>("neoLocalUserName");
        var neoPassword = config.GetSection("appSettings").GetValue<string>("neoLocalPassword");
        var neoUri = config.GetSection("appSettings").GetValue<string>("neoDevUrl");

        dbDriver = GraphDatabase.Driver(neoUri, AuthTokens.Basic(neoUserName, neoPassword));
  
        //dbClient = new GraphClient(new Uri("bolt://localhost:7687"), "Neo4j", "Neo4jPassword");     
        //dbClient.Connect();

    }

    public void Dispose()
    {
        dbDriver.CloseAsync();
    }

    public IDriver dbDriver { get; private set; }
    public IGraphClient dbClient {get; private set;}
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}