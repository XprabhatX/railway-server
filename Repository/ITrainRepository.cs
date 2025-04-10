using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Railway.Repository
{
    public interface ITrainRepository
    {
        Task<List<object>> SearchTrainsAsync(string fromStation, string toStation, DateTime date, float? minRating = null);
    }
}