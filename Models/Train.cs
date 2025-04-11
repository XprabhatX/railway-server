using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Railway.Models
{
    [Table("Trains")]
    public class Train
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TrainID { get; set; }

        [Required]
        [MaxLength(100)]
        public string TrainName { get; set; }

        [Required]
        [MaxLength(50)]
        public string TrainType { get; set; }

        [Required]
        public int TotalSeats { get; set; }

        [Required]
        [MaxLength(50)]
        public string RunningDays { get; set; }
    }
}