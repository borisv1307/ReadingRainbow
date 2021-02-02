using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text;
using Neo4jClient;
using System;

namespace ReadingRainbowAPI.DAL
{
    public interface INeo4jDBContext
    {
        GraphClient GetClient();   
  
    }
}