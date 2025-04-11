using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Authentication.Models;
using Railway.Models;

namespace Authentication.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<TrainSchedule> TrainSchedules { get; set; }
        public DbSet<ClassType> ClassTypes { get; set; }
        public DbSet<SeatAvailability> SeatAvailabilities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Train>().HasKey(t => t.TrainID);
            builder.Entity<TrainSchedule>().HasKey(ts => ts.ScheduleID);
            builder.Entity<Station>().HasKey(s => s.StationID);
            builder.Entity<ClassType>().HasKey(ct => ct.ClassTypeID);
            builder.Entity<SeatAvailability>().HasKey(sa => sa.AvailabilityID);
            builder.Entity<Ticket>().HasKey(t => t.TicketID);
            builder.Entity<Passenger>().HasKey(p => p.PassengerID);
        }
    }
}