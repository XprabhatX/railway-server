using Railway.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Railway.Repository
{
    public interface ITicketRepository
    {
        Task<bool> BookTicketAsync(Ticket ticket, List<Passenger> passengers);
    }
}
