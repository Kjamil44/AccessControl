using AccessControl.API.DomainEvents;
using Marten.Schema;
using System.Text.Json.Serialization;

namespace AccessControl.API.Models
{
    public class Lock : AggregateRoot
    {
        [ForeignKey(typeof(Site))]
        public Guid SiteId { get; set; }
        [Identity]
        public Guid LockId { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool IsLocked { get; set; }

        [JsonInclude]
        public List<AllowedUser> AllowedUsers { get; private set; } = [];

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
            AllowedUsers.Add(allowedUser);
        }

        private void EditAccess(AllowedUser allowedUser)
        {
            AllowedUsers
                .FirstOrDefault(x => x.CardholderId == allowedUser.CardholderId)
                .ScheduleId = allowedUser.ScheduleId;
        }

        private void RemoveAccess(AllowedUser allowedUser)
        {
            AllowedUsers.Remove(allowedUser);  
        }

        public void TriggerLock(string cardNumber, bool isAllowed = true, string reason = "")
        {
            DateModified = DateTime.UtcNow;
            IsLocked = true;

            Raise(new LockTriggeredDomainEvent(LockId, cardNumber, isAllowed, reason));
        }

        public void TriggerUnlock(string cardNumber, bool isAllowed = true, string reason = "")
        {
            DateModified = DateTime.UtcNow;
            IsLocked = false;

            Raise(new UnlockTriggeredDomainEvent(LockId, cardNumber, isAllowed, reason));
        }
    }
    public class AllowedUser
    {
        public Guid CardholderId { get; set; }
        public Guid ScheduleId { get; set; }
    }
}
