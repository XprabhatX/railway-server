using Microsoft.AspNetCore.Mvc;
using Railway.Repository;
using Railway.DTOs;

namespace Railway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class TrainController : ControllerBase
    {
        private readonly ITrainRepository _trainRepository;

        public TrainController(ITrainRepository trainRepository)
        {
            _trainRepository = trainRepository;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchTrains([FromBody] TrainSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FromStation) || string.IsNullOrWhiteSpace(request.ToStation))
            {
                return BadRequest("Both FromStation and ToStation are required.");
            }

            var result = await _trainRepository.SearchTrainsAsync(
                request.FromStation,
                request.ToStation,
                request.Date,
                request.MinRating
            );

            if (result == null || result.Count == 0)
                return NotFound("No trains found for the given criteria.");

            return Ok(result);
        }
    }
}
