using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTrackerBackend.Data.Models
{
    [Table("TimeEntries")]
    [PrimaryKey(nameof(TimeEntryId))]
    public class TimeEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeEntryId { get; set; }

        [ForeignKey(nameof(TaskId))]
        public int TaskId {  get; set; }

        [ForeignKey(nameof(UserId))]
        public int UserId {  get; set; }

        public TimeOnly TimeSpent { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}
