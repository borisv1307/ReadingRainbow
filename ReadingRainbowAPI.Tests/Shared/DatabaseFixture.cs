using Neo4j.Driver;
using Neo4jClient;
using System;
using Xunit;
using Microsoft.Extensions.Configuration;
using ReadingRainbowAPI.DAL;

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

        dbContext = new Neo4jDBContext();

    }

    public void Dispose()
    {

    }

    public INeo4jDBContext dbContext {get; private set;}
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}