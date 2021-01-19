using System;
using Microsoft.Extensions.Configuration;


namespace ReadingRainbowAPI.Shared
{
    

public class NeoCredentials : IDisposable
{

    public readonly string NeoUserName;
    public readonly string NeoPassword;
    public readonly string NeoUri;

        public NeoCredentials()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        NeoUserName = config.GetSection("NeO4jConnectionSettings").GetValue<string>("NeoUserName");
        NeoPassword = config.GetSection("NeO4jConnectionSettings").GetValue<string>("NeoPW");
        NeoUri = config.GetSection("NeO4jConnectionSettings").GetValue<string>("NeoURI");

        }


    public void Dispose()
    {

    }

}

}