using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDBWebApi.Database.Models;
using MongoDBWebApi.Database.Repositories.Base;

namespace MongoDBWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IBaseRepository<Movie> _movieRepository;

        public MovieController(ILogger<MovieController> logger, IBaseRepository<Movie> movieRepository)
        {
            _logger = logger;
            _movieRepository = movieRepository;
        }

        [HttpGet("getMovies")]
        public IEnumerable<Movie> Get()
        {
            var movies = _movieRepository.GetBy(x => x.name.Equals("test"));
            return movies;
        }

        [HttpPost("saveMovie")]
        public async Task AddPerson(string movieName)
        {
            var movie = new Movie()
            {
                name = movieName
            };

            await _movieRepository.SaveAsync(movie);
        }
    }
}
