using Microsoft.AspNetCore.Mvc;
using TimeTrackerBackend.Data;
using TimeTrackerBackend.Data.Models;

namespace TimeTrackerBackend.Controllers
{
    [ApiController]
    [Route("/[controller]/")]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        [Route("{userName}")]
        public IActionResult GetAllByUserId(string userName, TimeTrackerDbContext context)
        {
            var foundUser = context.Users.FirstOrDefault(user => user.UserName == userName);

            var result = (foundUser is null)
                ? new List<TimeEntry>()
                : context.TimeEntries.Where(entry => entry.UserId == foundUser.UserId).ToList();

            return Ok(new JsonResult(result));
        }

        [HttpGet]
        [Route("avg/{userName}")]
        public IActionResult GetAvgByUserId(string userName, TimeTrackerDbContext context)
        {
            var foundUser = context.Users.FirstOrDefault(user => user.UserName == userName);

            var result = (foundUser is null)
                ? new List<TimeEntry>()
                : context.TimeEntries.Where(entry => entry.UserId == foundUser.UserId).ToList();

            return Ok(new JsonResult(new
            { 
                EntryCount = result.Count(),
                TaskCount = result.Select(x => x.TaskId).Distinct().Count(),
                TotalTime = result.Select(x => x.TimeSpent).Aggregate((x1, x2) => TimeOnly.FromTimeSpan(x1.ToTimeSpan() + x2.ToTimeSpan())),
                TotalDays = result.Select(x => x.RecordedDate.Date).Distinct().Count(),
                LastTrackedDay = result.Select(x => x.RecordedDate.Date).Max()
            }));
        }

        [HttpGet]
        [Route("listtasks/{userName}")]
        public IActionResult GetTaskListByUser(string userName, TimeTrackerDbContext context)
        {
            var foundUser = context.Users.FirstOrDefault(user => user.UserName == userName);
            var result = (foundUser is null)
                ? new List<string>()
                : context.TimeEntries.Where(entry => entry.UserId == foundUser.UserId).Select(x => x.TaskId)
                  .Join(context.Tasks, item => item, taskItem => taskItem.TaskId, (id, elem) => elem.Name)
                  .Distinct()
                  .ToList();

            return Ok(new JsonResult(result));
        }

        [HttpGet]
        [Route("{userName}/{taskName}")]
        public IActionResult GetAllByUserIdAndTaskId(string userName, string taskName, TimeTrackerDbContext context)
        {
            var foundUser = context.Users.FirstOrDefault(user => user.UserName == userName);
            var foundTask = context.Tasks.FirstOrDefault(task => task.Name == taskName);

            var result = (foundUser is null || foundTask is null)
                ? new List<TimeEntry>()
                : context.TimeEntries.Where(entry => entry.UserId == foundUser.UserId && entry.TaskId == foundTask.TaskId)
                  .ToList();

            return Ok(new JsonResult(result));
        }

        [HttpGet]
        [Route("avg/{userName}/{taskName}")]
        public IActionResult GetAvgByUserIdAndTaskId(string userName, string taskName, TimeTrackerDbContext context)
        {
            var foundUser = context.Users.FirstOrDefault(user => user.UserName == userName);
            var foundTask = context.Tasks.FirstOrDefault(task => task.Name == taskName);

            if (foundUser is null || foundTask is null)
                return BadRequest();

            var result = (foundUser is null || foundTask is null)
                ? new List<TimeEntry>()
                : context.TimeEntries.Where(entry => entry.UserId == foundUser.UserId && entry.TaskId == foundTask.TaskId)
                  .ToList();

            return Ok(new JsonResult(new
            {
                EntryCount = result.Count,
                TotalTime = result.Select(x => x.TimeSpent).Aggregate((x1, x2) => TimeOnly.FromTimeSpan(x1.ToTimeSpan() + x2.ToTimeSpan())),
                TotalDays = result.Select(x => x.RecordedDate.Date).Distinct().Count(),
                LastTrackedDay = result.Select(x => x.RecordedDate.Date).Max()
            }));
        }
    }
}
