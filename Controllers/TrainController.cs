using Microsoft.AspNetCore.Mvc;
using Railway.Repository;
using Railway.DTOs;
using Railway.Models;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddTrain([FromBody] TrainCreationRequest trainRequest)
        {
            var train = new Train
            {
                TrainName = trainRequest.TrainName,
                TrainType = trainRequest.TrainType,
                TotalSeats = trainRequest.TotalSeats,
                RunningDays = trainRequest.RunningDays
            };

            var schedules = new List<TrainSchedule>();
            foreach (var scheduleDto in trainRequest.Schedules)
            {
                schedules.Add(new TrainSchedule
                {
                    StationID = scheduleDto.StationID,
                    ArrivalTime = scheduleDto.ArrivalTime,
                    DepartureTime = scheduleDto.DepartureTime,
                    SequenceOrder = scheduleDto.SequenceOrder,
                    Fair = scheduleDto.Fair,
                    DistanceFromSource = scheduleDto.DistanceFromSource
                });
            }

            var success = await _trainRepository.AddTrainAsync(train, schedules);
            if (success)
                return Ok("Train added successfully.");
            else
                return BadRequest("Failed to add train.");
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTrain([FromBody] TrainUpdateRequest updateRequest)
        {
            var train = new Train
            {
                TrainID = updateRequest.TrainID,
                TrainName = updateRequest.TrainName,
                TrainType = updateRequest.TrainType,
                TotalSeats = updateRequest.TotalSeats,
                RunningDays = updateRequest.RunningDays
            };

            var updatedSchedules = new List<TrainSchedule>();
            foreach (var scheduleDto in updateRequest.Schedules)
            {
                updatedSchedules.Add(new TrainSchedule
                {
                    StationID = scheduleDto.StationID,
                    ArrivalTime = scheduleDto.ArrivalTime,
                    DepartureTime = scheduleDto.DepartureTime,
                    SequenceOrder = scheduleDto.SequenceOrder,
                    Fair = scheduleDto.Fair,
                    DistanceFromSource = scheduleDto.DistanceFromSource
                });
            }

            var success = await _trainRepository.UpdateTrainAsync(train, updatedSchedules);
            if (success)
                return Ok("Train updated successfully.");
            else
                return BadRequest("Failed to update train. Ensure the train exists.");
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTrains()
        {
            var result = await _trainRepository.GetAllTrainsAsync();
            if (result == null || !result.Any())
                return NotFound("No trains found.");
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTrainById(int id)
        {
            var train = await _trainRepository.GetTrainByIdAsync(id);
            if (train == null)
                return NotFound("Train not found.");
            return Ok(train);
        }

        [HttpPut("updateScheduleForDate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTrainScheduleForDate([FromBody] ScheduleUpdateForDateRequest request)
        {
            var success = await _trainRepository.UpdateScheduleForSpecificDateAsync(request);

            if (success)
                return Ok("Train schedule updated for specific date.");
            else
                return BadRequest("Failed to update schedule.");
        }
    }
}
