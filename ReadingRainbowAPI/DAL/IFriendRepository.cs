using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Relationships;

namespace ReadingRainbowAPI.DAL
{

    public interface IFriendRepository
    {       
        Task<IEnumerable<Person>> GetConfirmedFriendsWithRelationshipAsync(Person person, FriendsWith friendsWith);
        Task<IEnumerable<Person>> GetFriendRequests(Person person, FriendsWith friendsWith);
        Task<IEnumerable<Person>> GetRequestedFriends(Person person, FriendsWith friendsWith);

        Task DeleteFriendsWithRelationshipAsync(Person person1, Person person2);
    }
}