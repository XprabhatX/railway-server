using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Railway.DTOs;
using Railway.Models;
using Railway.Repository;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Railway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Roles = "User")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [HttpPost("book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookTicket([FromBody] TicketBookingRequest request)
        {
            var ticket = new Ticket
            {
                Username = User.Identity.Name,
                TrainID = request.TrainID,
                SourceID = request.SourceID,
                DestinationID = request.DestinationID,
                JourneyDate = request.JourneyDate,
                ClassTypeID = request.ClassTypeID,
                HasInsurance = request.HasInsurance
            };

            var passengers = new List<Passenger>();
            foreach (var passengerDto in request.Passengers)
            {
                passengers.Add(new Passenger
                {
                    Name = passengerDto.Name,
                    Gender = passengerDto.Gender,
                    Age = passengerDto.Age,
                    SeatNumber = passengerDto.SeatNumber
                });
            }

            bool success = await _ticketRepository.BookTicketAsync(ticket, passengers);
            if (success)
                return Ok("Ticket booked successfully. Here is ");
            else
                return BadRequest("Failed to book ticket. Possibly insufficient seats or schedule data issue.");
        }
    }
}
