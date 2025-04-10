namespace Railway.Models
{
    public class SeatAvailability
    {
        public int AvailabilityID {get; set;}
        public int TrainID {get; set;}
        public DateTime Date {get; set;}
        public int ClassTypeID {get; set;}
        public int RemainingSeats {get; set;}
    }
}