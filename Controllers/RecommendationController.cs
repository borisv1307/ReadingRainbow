using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Relationships;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using ReadingRainbowAPI.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace ReadingRainbowAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/recommendations")]

    public class RecommendationController : ControllerBase
    {
        // private readonly BookRepository _bookRepository;
        // private readonly PersonRepository _personRepository;
        private readonly RecommendationRepository _recommendationRepository;

        private readonly IMapper _mapper;

// BookRepository bookRepository, GenreRepository genreRepository, PersonRepository personRepository,
        public RecommendationController(RecommendationRepository recommendationRepository, IMapper mapper)
        {
            // _bookRepository = bookRepository;
            // _personRepository = personRepository;
            _recommendationRepository = recommendationRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Popular/WishList")]
        public async Task<IActionResult> RequestPopWish()
        {
             var popWishList = (await _recommendationRepository.PopularWishLists()).ToList();
             return Ok(JsonSerializer.Serialize(popWishList));
        }

        [HttpGet]
        [Route("Popular/Library")]
        public async Task<IActionResult> RequestPopLib()
        {
             var popLibrary = (await _recommendationRepository.PopularLibrarys()).ToList();
             return Ok(JsonSerializer.Serialize(popLibrary));
        }

        // [HttpGet]
        // [Route("Jaccard/WishList")]
        // public async Task<IActionResult>
        // {
        // }

        // [HttpGet]
        // [Route("Jaccard/Library")]
        // public async Task<IActionResult>
        // {
        // }
    }
}