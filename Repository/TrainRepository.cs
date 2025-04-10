using Authentication.Data;
using Microsoft.EntityFrameworkCore;
using Railway.Models;

namespace Railway.Repository
{
    public class TrainRepository : ITrainRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<object>> SearchTrainsAsync(string fromStation, string toStation, DateTime date, float? minRating = null)
        {
            var source = await _context.Stations.FirstOrDefaultAsync(s => s.StationName == fromStation);
            var destination = await _context.Stations.FirstOrDefaultAsync(s => s.StationName == toStation);
            if (source == null || destination == null) return new List<object>();

            var allSchedules = await _context.TrainSchedules
                .Include(ts => ts.Train)
                .Where(ts => ts.StationID == source.StationID || ts.StationID == destination.StationID)
                .OrderBy(ts => ts.SequenceOrder)
                .ToListAsync();

            var classTypes = await _context.ClassTypes.ToDictionaryAsync(ct => ct.ClassTypeID, ct => ct.ClassName);

            var seatAvailability = await _context.SeatAvailabilities
                .Where(sa => sa.Date.Date == date.Date)
                .ToListAsync();

            var result = allSchedules
                .GroupBy(ts => ts.TrainID)
                .Select(group =>
                {
                    var schedules = group.ToList();
                    var sourceStop = schedules.FirstOrDefault(s => s.StationID == source.StationID);
                    var destStop = schedules.FirstOrDefault(s => s.StationID == destination.StationID);

                    if (sourceStop == null || destStop == null || sourceStop.SequenceOrder >= destStop.SequenceOrder)
                        return null;

                    var train = group.First().Train;

                    var seatsForTrain = seatAvailability
                        .Where(sa => sa.TrainID == train.TrainID)
                        .GroupBy(sa => sa.ClassTypeID)
                        .ToDictionary(
                            g => classTypes.ContainsKey(g.Key) ? classTypes[g.Key] : $"Class-{g.Key}",
                            g => g.Sum(sa => sa.RemainingSeats)
                        );

                    return new
                    {
                        TrainID = train.TrainID,
                        TrainName = train.TrainName,
                        AvailableSeats = seatsForTrain,
                        Rating = 4.5f
                    };
                })
                .Where(t => t != null && (minRating == null || t.Rating >= minRating))
                .Cast<object>()
                .ToList();

            return result;
        }

        public async Task<bool> AddTrainAsync(Train train, List<TrainSchedule> schedule)
        {
            await _context.Trains.AddAsync(train);
            await _context.SaveChangesAsync();

            foreach (var item in schedule)
                item.TrainID = train.TrainID;

            await _context.TrainSchedules.AddRangeAsync(schedule);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateTrainAsync(Train updatedTrain, List<TrainSchedule> updatedSchedules)
        {
            var existingTrain = await _context.Trains.FindAsync(updatedTrain.TrainID);
            if (existingTrain == null) 
                return false;

            existingTrain.TrainName = updatedTrain.TrainName;
            existingTrain.TrainType = updatedTrain.TrainType;
            existingTrain.TotalSeats = updatedTrain.TotalSeats;
            existingTrain.RunningDays = updatedTrain.RunningDays;
            
            _context.Trains.Update(existingTrain);

            var oldSchedules = _context.TrainSchedules.Where(s => s.TrainID == updatedTrain.TrainID);
            _context.TrainSchedules.RemoveRange(oldSchedules);
            
            foreach(var schedule in updatedSchedules)
            {
                schedule.TrainID = updatedTrain.TrainID;
            }
            await _context.TrainSchedules.AddRangeAsync(updatedSchedules);

            return await _context.SaveChangesAsync() > 0;
        }

    }
}
