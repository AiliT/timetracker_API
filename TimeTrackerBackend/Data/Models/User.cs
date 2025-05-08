using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrackerBackend.Data.Models
{
    [Table("Users")]
    [PrimaryKey(nameof(UserId))]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<TimeTask> Tasks { get; set; }
        public List<TimeEntry> TimeEntries { get; set; }
    }
}
