
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;
using Neo4jClient;

namespace ReadingRainbowAPI.DAL
{

public class FriendRepository: IFriendRepository
{
    
        protected readonly IGraphClient _neoContext;
 
        public FriendRepository(INeo4jDBContext context)
        {
             _neoContext = context.GetClient(); 
        }

        public async Task<IEnumerable<Person>> GetConfirmedFriendsWithRelationshipAsync(Person person, FriendsWith friendsWith)
        {
            // Get list of all requests that have <- r -> because they are confirmed both ways
            return await this.GetDualRelated(p1=>p1.Name == person.Name, friendsWith);
        }

        public async Task<IEnumerable<Person>> GetFriendRequests(Person person, FriendsWith friendsWith)
        {
            // Get list of all requests coming in for person <- r - but not - r ->
            return await this.GetLeftRelated(p1=>p1.Name == person.Name);
        }

        public async Task<IEnumerable<Person>> GetRequestedFriends(Person person, FriendsWith friendsWith)
        {
            // Get list of all requests going out from person - r -> but not <- r -
            return await this.GetRightRelated(p1=>p1.Name == person.Name);
        }

        public async Task DeleteFriendsWithRelationshipAsync(Person person1, Person person2)
        {
            await this.DeleteFriendRelationship(p1=>p1.Name == person1.Name, p2=>p2.Name == person2.Name);
        }
    
        private async Task<IEnumerable<Person>> GetDualRelated<FriendsWith>(Expression<Func<Person, bool>> query, FriendsWith relationship)
        {
            try{
            return  await _neoContext.Cypher
                .Match("(p1:Person)-[:FRIENDS_WITH]->(p2:Person)")
                .Where("(p2:Person)-[:FRIENDS_WITH]->(p1:Person)") 
                .AndWhere(query)
                .Return<Person>("p2")
                .ResultsAsync;
           } catch (Exception ex)
           {
                Console.WriteLine($"exception occured {ex}");
                return new List<Person>();
           }
        }

        private async Task<IEnumerable<Person>> GetRightRelated(Expression<Func<Person, bool>> query1)
        {
            try{
                        return await _neoContext.Cypher
                            .Match("(p1:Person)-[:FRIENDS_WITH]->(p2:Person)")
                            .Where("NOT((p2:Person)-[:FRIENDS_WITH]->(p1:Person))")
                            .AndWhere(query1)
                            .Return<Person>("p2")
                            .ResultsAsync;
            } catch (Exception)
            {
              return new List<Person>();
            }

        }

        
        private async Task<IEnumerable<Person>> GetLeftRelated(Expression<Func<Person, bool>> query1)
        {

            try{
            return await _neoContext.Cypher
                .Match("(p2:Person)-[:FRIENDS_WITH]->(p1:Person)")
                .Where("NOT((p1:Person)-[:FRIENDS_WITH]->(p2:Person))")
                .AndWhere(query1)
                .Return<Person>("p2")
                .ResultsAsync;
            } catch (Exception)
            {
                return new List<Person>();
            }
        }

        private async Task<bool> DeleteFriendRelationship(Expression<Func<Person, bool>> query1, Expression<Func<Person, bool>> query2)
        {
            await _neoContext.Cypher
                .Match("(p1:Person)-[r:FRIENDS_WITH]->(p2:Person)")
                .Where(query1)
                .AndWhere(query2)
                .Delete("r")
                .ExecuteWithoutResultsAsync();

            await _neoContext.Cypher
                .Match("(p2:Person)-[r:FRIENDS_WITH]->(p1:Person)")
                .Where(query1)
                .AndWhere(query2)
                .Delete("r")
                .ExecuteWithoutResultsAsync();

            return true;
        }


}
}
