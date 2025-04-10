namespace Railway.DTOs
{
    public class TrainUpdateRequest
    {
        public int TrainID { get; set; }
        // Train details
        public string TrainName { get; set; }
        public string TrainType { get; set; }
        public int TotalSeats { get; set; }
        public string RunningDays { get; set; }        
        public List<TrainScheduleDTO> Schedules { get; set; }
    }
}