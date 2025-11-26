using AccessControl.API.Enums;

namespace AccessControl.API.Helpers
{
    public static class LiveEventMapping
    {
        public static LiveEventType ToCategory(this LiveEventMessageType t) => t switch
        {
            LiveEventMessageType.LockTriggerGranted or
            LiveEventMessageType.LockTriggerDenied or
            LiveEventMessageType.UnlockTriggerGranted or
            LiveEventMessageType.UnlockTriggerDenied => LiveEventType.LockUnlock,

            LiveEventMessageType.SiteCreated or
            LiveEventMessageType.SiteDeleted or
            LiveEventMessageType.SiteUpdated => LiveEventType.SiteOperations,

            LiveEventMessageType.LockCreated or
            LiveEventMessageType.LockDeleted or
            LiveEventMessageType.LockUpdated or
            LiveEventMessageType.LockAccessListUpdated => LiveEventType.LockOperations,

            LiveEventMessageType.CardholderCreated or
            LiveEventMessageType.CardholderDeleted or
            LiveEventMessageType.CardholderUpdated => LiveEventType.CardholderOperations,

            LiveEventMessageType.ScheduleCreated or
            LiveEventMessageType.ScheduleDeleted or
            LiveEventMessageType.ScheduleUpdated => LiveEventType.ScheduleOperations,

            _ => LiveEventType.None
        };
    }
}
