using System;
using System.Collections.Generic;
using Railway.Models;

namespace Railway.DTOs
{
    public class TrainCreationRequest
    {
        // Train info
        public string TrainName { get; set; }
        public string TrainType { get; set; }
        public int TotalSeats { get; set; }
        public string RunningDays { get; set; }  // e.g., "Mon,Tue,Wed,..."
        
        // List of schedule entries for the route
        public List<TrainScheduleDTO> Schedules { get; set; }
    }

    public class TrainScheduleDTO
    {
        // For a schedule, you will send:
        public int StationID { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public int SequenceOrder { get; set; }
        public float Fair { get; set; }
        public int DistanceFromSource { get; set; }
    }
}
