using System;
using System.Collections.Generic;
using Railway.Models;

namespace Railway.DTOs
{
    public class ScheduleUpdateForDateRequest
    {
        public int TrainID { get; set; }
        public DateTime Date { get; set; }
        public List<SeatUpdateDto> Updates { get; set; }
        public List<TrainSchedule> NewSchedules { get; set; }
    }

    public class SeatUpdateDto
    {
        public int ClassTypeID { get; set; }
        public int RemainingSeats { get; set; }
    }

}