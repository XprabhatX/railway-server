using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Railway.Models;
using Railway.DTOs;

namespace Railway.Repository
{
    public interface ITrainRepository
    {
        Task<List<object>> SearchTrainsAsync(string fromStation, string toStation, DateTime date, float? minRating = null);
        Task<bool> AddTrainAsync(Train train, List<TrainSchedule> schedule);
        Task<bool> UpdateTrainAsync(Train updatedTrain, List<TrainSchedule> updatedSchedules);
        Task<List<object>> GetAllTrainsAsync();
        Task<object?> GetTrainByIdAsync(int trainId);
        Task<bool> UpdateScheduleForSpecificDateAsync(ScheduleUpdateForDateRequest request);
    }
}