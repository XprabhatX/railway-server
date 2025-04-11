using Authentication.Data;
using Microsoft.EntityFrameworkCore;
using Railway.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Railway.Repository
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BookTicketAsync(Ticket ticket, List<Passenger> passengers)
        {
            if (!int.TryParse(ticket.DestinationID, out int destinationID))
            {
                return false;
            }

            var sourceSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
                ts.TrainID == ticket.TrainID && ts.StationID == ticket.SourceID);

            var destinationSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
                ts.TrainID == ticket.TrainID && ts.StationID == destinationID);

            if (sourceSchedule == null || destinationSchedule == null ||
                sourceSchedule.SequenceOrder >= destinationSchedule.SequenceOrder)
            {
                return false;
            }

            float baseFare = destinationSchedule.Fair - sourceSchedule.Fair;
            var classType = await _context.ClassTypes.FirstOrDefaultAsync(ct => ct.ClassTypeID == ticket.ClassTypeID);
            decimal multiplier = 1.0m;
            if (classType != null)
            {
                switch (classType.ClassName)
                {
                    case "AC First Class":
                        multiplier = 2.0m;
                        break;
                    case "AC 2 Tier":
                        multiplier = 1.8m;
                        break;
                    case "AC 3 Tier":
                        multiplier = 1.5m;
                        break;
                    default:
                        multiplier = 1.0m;
                        break;
                }
            }

            decimal farePerPassenger = (decimal)baseFare * multiplier;
            ticket.Fare = farePerPassenger * passengers.Count;

            var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
                sa.TrainID == ticket.TrainID &&
                sa.Date.Date == ticket.JourneyDate.Date &&
                sa.ClassTypeID == ticket.ClassTypeID);

            int seatsNeeded = passengers.Count;
            if (seatAvailability == null || seatAvailability.RemainingSeats < seatsNeeded)
            {
                return false;
            }

            seatAvailability.RemainingSeats -= seatsNeeded;

            ticket.BookingDate = DateTime.Now;
            ticket.Status = "Booked";

            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();

            foreach (var passenger in passengers)
            {
                passenger.TicketID = ticket.TicketID;
            }
            await _context.Passengers.AddRangeAsync(passengers);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
