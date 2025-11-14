using AccessControl.API.Enums;
using Marten.Schema;

namespace AccessControl.API.Models
{
    public class LiveEvent
    {
        [ForeignKey(typeof(User))]
        public Guid UserId { get; set; }
        public Guid SiteId { get; set; }
        public Guid EntityId {  get; set; }
        [Identity]
        public Guid LiveEventId { get; set; }
        public string EntityType { get; set; }
        public LiveEventType Type { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public LiveEventMessageType MessageType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
