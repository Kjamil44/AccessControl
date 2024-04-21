using AccessControl.API.Enums;
using Marten.Schema;

namespace AccessControl.API.Models
{
    public class Schedule
    {
        [ForeignKey(typeof(Site))]
        public Guid SiteId { get; set; }
        [Identity]
        public Guid ScheduleId { get; set; }
        public IEnumerable<Days> ListOfDays { get; set; } = new List<Days>();
        public string DisplayName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Schedule(Guid siteId, IEnumerable<Days> listOfDays, string displayName, DateTime startTime, DateTime endTime)
        {
            SiteId = siteId;
            ListOfDays = listOfDays;
            DisplayName = displayName;
            StartTime = startTime;
            EndTime = endTime;
            DateCreated = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }
        public void UpdateSchedule(IEnumerable<Days> listOfDays, string displayName, DateTime startTime, DateTime endTime)
        {
            ListOfDays = listOfDays;
            DisplayName = displayName;
            StartTime = startTime;
            EndTime = endTime;
            DateModified = DateTime.UtcNow;
        }
    }
}
