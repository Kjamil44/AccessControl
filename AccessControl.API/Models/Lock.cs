using Baseline;
using Marten.Schema;

namespace AccessControl.API.Models
{
    public class Lock
    {
        [ForeignKey(typeof(Site))]
        public Guid SiteId { get; set; }
        [Identity]
        public Guid LockId { get; set; }
        public string DisplayName { get; set; }
        public IEnumerable<AllowedUser> AllowedUsers => _allowedUsers;
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        private readonly IList<AllowedUser> _allowedUsers = new List<AllowedUser>();
        public Lock(Guid siteId, string displayName)
        {
            SiteId = siteId;
            DisplayName = displayName;
            DateCreated = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }
        public void UpdateLock(string displayName)
        {
            DisplayName = displayName;
            DateModified = DateTime.UtcNow;
        }
        public void AssignAccessToLock(AllowedUser allowedUser)
        {
            AddAccess(allowedUser);
        }
        public void EditAccessToLock(AllowedUser allowedUser)
        {
            EditAccess(allowedUser);
        }
        public void RemoveAccessFromLock(AllowedUser allowedUser)
        {
            RemoveAccess(allowedUser);
        }
        private void AddAccess(AllowedUser allowedUser)
        {
            _allowedUsers.Add(allowedUser);
        }
        private void EditAccess(AllowedUser allowedUser)
        {
            _allowedUsers
                .FirstOrDefault(x => x.CardholderId == allowedUser.CardholderId)
                .ScheduleId = allowedUser.ScheduleId;
        }
        private void RemoveAccess(AllowedUser allowedUser)
        {
            _allowedUsers.Remove(allowedUser);  
        }
    }
    public class AllowedUser
    {
        public Guid CardholderId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}
