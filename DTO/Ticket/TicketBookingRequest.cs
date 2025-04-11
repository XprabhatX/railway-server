using System;
using System.Collections.Generic;

namespace Railway.DTOs
{
    public class TicketBookingRequest
    {
        public int TrainID { get; set; }
        public int SourceID { get; set; }
        public string DestinationID { get; set; }
        public DateTime JourneyDate { get; set; }
        public int ClassTypeID { get; set; }
        public bool HasInsurance { get; set; }
        public List<PassengerBookingDto> Passengers { get; set; }
    }

    public class PassengerBookingDto
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string SeatNumber { get; set; }
    }
}