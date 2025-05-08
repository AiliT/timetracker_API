using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrackerBackend.Data.Models
{
    [Table("Tasks")]
    [PrimaryKey(nameof(TaskId))]
    public class TimeTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }
        public string Name { get; set; }
    }
}
