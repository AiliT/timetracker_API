using Microsoft.AspNetCore.Mvc;
using TimeTrackerBackend.Data;
using TimeTrackerBackend.Models;
using TimeTrackerBackend.Data.Models;

namespace TimeTrackerBackend.Controllers
{
    [ApiController]
    [Route("/[controller]/")]
    public class TrackerController : ControllerBase
    {
        [HttpPost]
        [Route("track")]
        public async Task<ActionResult> PostTrackingDataAsync(TrackingDataModel data, TimeTrackerDbContext context)
        {
            if (string.IsNullOrEmpty(data.UserName) || string.IsNullOrEmpty(data.TaskName))
                return BadRequest();

            var foundUser = context.Users.FirstOrDefault(user => user.UserName == data.UserName);
            if (foundUser is null)
            {
                foundUser = (await context.Users.AddAsync(new User { UserName = data.UserName })).Entity;
            }

            var foundTask = context.Tasks.FirstOrDefault(task => task.Name == data.TaskName);
            if (foundTask is null)
            {
                foundTask = (await context.Tasks.AddAsync(new TimeTask { Name = data.TaskName })).Entity;
            }

            if (!TimeOnly.TryParseExact(data.TimeTracked, "HH:mm:ss", out var parsedTime))
            {
                return BadRequest();
            }

            await context.SaveChangesAsync();
            await context.TimeEntries.AddAsync(new TimeEntry
            {
                UserId = foundUser.UserId,
                TaskId = foundTask.TaskId,
                TimeSpent = parsedTime,
                RecordedDate = DateTime.UtcNow
            });

            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
