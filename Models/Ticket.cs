namespace Railway.Models
{
    public class Ticket
    {
        public int TicketID {get; set;}
        public string Username {get; set;}
        public int TrainID {get; set;}
        public int SourceID {get; set;}
        public string DestinationID {get; set;}
        public DateTime BookingDate {get; set;}
        public DateTime JourneyDate {get; set;}
        public int ClassTypeID {get; set;}
        public decimal Fare {get; set;}
        public string Status {get; set;}
        public bool HasInsurance {get; set;}

    }
}