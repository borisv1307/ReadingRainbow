using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Middleware;
using ReadingRainbowAPI.Relationships;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ReadingRainbowAPI.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System;
using System.Text.RegularExpressions;

namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/friend")]
    public class FriendController : ControllerBase
    {

        private readonly PersonRepository _personRepository;
        private readonly IFriendRepository _friendRepository;

        private readonly IMapper _mapper;
 
        public FriendController(PersonRepository personRepository, IFriendRepository friendRepository,
        IMapper mapper)
        {
            _personRepository = personRepository;
            _friendRepository = friendRepository;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("RequestFriend")]
        public async Task<IActionResult> RequestFriend(string userName, string friendName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            // Check Friend
            var friend = await _personRepository.GetPersonAsync(friendName);
            if (friend == null)
            {
                return Ok($"friend {friendName} could not be found");
            }

            // Request friend
            await _personRepository.CreateFriendRelationshipAsync(user, friend, new FriendsWith());

            return Ok("Friend Request Sent");
        }

        [HttpGet]
        [Route("GetFriends/{username}")]
        public async Task<IActionResult> GetFriends(string userName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            var friends = (await _friendRepository.GetConfirmedFriendsWithRelationshipAsync(user, new FriendsWith())).ToList();
            var friendsDto = _mapper.Map<List<Person>, List<PersonDto>>(friends);

            return Ok(JsonSerializer.Serialize(friendsDto));
        }

        [HttpGet]
        [Route("GetFriendRequests/{username}")]
        public async Task<IActionResult> GetFriendRequests(string userName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            var friends = (await _friendRepository.GetFriendRequests(user, new FriendsWith())).ToList();
            var friendsDto = _mapper.Map<List<Person>, List<PersonDto>>(friends);

            return Ok(JsonSerializer.Serialize(friendsDto));
        }

        [HttpGet]
        [Route("GetRequestedFriends/{username}")]
        public async Task<IActionResult> GetRequestedFriends(string userName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            var friends = (await _friendRepository.GetRequestedFriends(user, new FriendsWith())).ToList();
            var friendsDto = _mapper.Map<List<Person>, List<PersonDto>>(friends);

            return Ok(JsonSerializer.Serialize(friendsDto));
        }

        [HttpPost]
        [Route("ConfirmFriendRequest")]
        public async Task<IActionResult> ConfirmFriendRequest(string userName, string friendName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            // Check Friend
            var friend = await _personRepository.GetPersonAsync(friendName);
            if (friend == null)
            {
                return Ok($"friend {friendName} could not be found");
            }

            // Request friend
            await _personRepository.CreateFriendRelationshipAsync(user, friend, new FriendsWith());

            return Ok("Friend Request Confirmed");
        }

        [HttpPost]
        [Route("RejectFriendRequest")]
        public async Task<IActionResult> RejectFriendRequest(string userName, string friendName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            // Check Friend
            var friend = await _personRepository.GetPersonAsync(friendName);
            if (friend == null)
            {
                return Ok($"friend {friendName} could not be found");
            }

            // Request friend
            await _friendRepository.DeleteFriendsWithRelationshipAsync(user, friend);

            return Ok("Friend Request Rejected");
        }

        [HttpPost]
        [Route("RemoveFriend")]
        public async Task<IActionResult> RemoveFriend(string userName, string friendName)
        {
            // Check user
            var user = await _personRepository.GetPersonAsync(userName);
            if (user == null)
            {
                return Ok($"user {userName} could not be found");
            }

            // Check Friend
            var friend = await _personRepository.GetPersonAsync(friendName);
            if (friend == null)
            {
                return Ok($"friend {friendName} could not be found");
            }

            // Request friend
            await _friendRepository.DeleteFriendsWithRelationshipAsync(user, friend);

            return Ok("Friend Removed");
        }
 
    }
}