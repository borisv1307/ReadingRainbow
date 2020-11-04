using Neo4j.Driver;
using Neo4jClient;
using RSG;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq.Expressions;
using System.Text;
using System;

namespace ReadingRainbowAPI.DAL
{
    public class Neo4jDBContext : INeo4jDBContext
    {
        public GraphClient dbClient {get; private set;}
 
        public Neo4jDBContext()
        {
            dbClient = new GraphClient(new Uri("http://localhost:7474"),"Neo4j","abc123");
            var running = true;

            ConnectToDatabase()
            .Then(result =>               
                {
                    Console.WriteLine("Async operation completed.");
                    Console.WriteLine($"database is {result}");
                    running = false;
                })
                .Done();

            while (running)
            {
                Thread.Sleep(10);
            }
        }

        private IPromise<string> ConnectToDatabase()
        {
            Console.WriteLine("Trying to connect to uri ...");

            var promise = new Promise<string>();
            var connectionCount = 0;
  
            do
            {
                dbClient.ConnectAsync();
                connectionCount += 1;
                Console.WriteLine($"connection test number {connectionCount}");

                WaitForDatabase();

                if (connectionCount > 20)
                    break;

            } while(dbClient.IsConnected == false);

            if (dbClient.IsConnected)
                promise.Resolve("connected");
            else
                promise.Reject(new SystemException());

            return promise;
        }

        private void WaitForDatabase()
        {
            var waitCount = 0;
                
                do
                {
                    waitCount += 1;
                    Thread.Sleep(200);
                    Console.WriteLine($"waiting for connection wait number {waitCount}");
                    if (waitCount > 20)
                        break;

                } while(dbClient.IsConnected == false);
        }

        public GraphClient GetClient()
        {
            return dbClient;
        }
}
      
}