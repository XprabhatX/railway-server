namespace Railway.Models
{
    public class TrainSchedule
    {
        public int ScheduleID {get; set;}
        public int TrainID {get; set;}
        public int StationID {get; set;}
        public DateTime ArrivalTime {get; set;}
        public DateTime DepartureTime {get; set;}
        public int SequenceOrder {get; set;}
        public float Fair {get; set;}
        public int DistanceFromSource {get; set;}

        public Train Train {get; set;}
        public Station Station {get; set;}
    }
}