using Neo4j.Driver;
using System;
using Xunit;

public class DatabaseFixture : IDisposable
{

    public DatabaseFixture()
    {
        var neoUserName = Environment.GetEnvironmentVariable("neoLocalUserName");
        var neoPassword = Environment.GetEnvironmentVariable("neoLocalPassword");

        dbDriver = GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("Neo4j", "Neo4jPassword"));
    }

    public void Dispose()
    {
        dbDriver.CloseAsync();
    }

    public IDriver dbDriver { get; private set; }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}