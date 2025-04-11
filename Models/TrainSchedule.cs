using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Railway.Models
{
    [Table("TrainSchedules")]
    public class TrainSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleID { get; set; }

        [Required]
        [ForeignKey("Train")]
        public int TrainID { get; set; }

        [Required]
        [ForeignKey("Station")]
        public int StationID { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public int SequenceOrder { get; set; }

        [Required]
        [Column(TypeName = "decimal(8, 2)")]
        public float Fair { get; set; }

        [Required]
        public int DistanceFromSource { get; set; }

        public Train Train { get; set; }
        public Station Station { get; set; }
    }
}