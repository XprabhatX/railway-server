namespace Railway.Models
{
    public class Ticket
    {
        int TicketID {get; set;}
        string Username {get; set;}
        int TrainID {get; set;}
        int SourceID {get; set;}
        string DestinationID {get; set;}
        DateTime BookingDate {get; set;}
        DateTime JourneyDate {get; set;}
        int ClassTypeID {get; set;}
        decimal Fare {get; set;}
        string Status {get; set;}
        bool HasInsurance {get; set;}

    }
}