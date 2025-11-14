namespace AccessControl.API.Enums
{
    public enum LiveEventMessageType
    {
        Unknown = 0,

        LockTriggerGranted = 1,
        LockTriggerDenied = 2,
        UnlockTriggerGranted = 3,
        UnlockTriggerDenied = 4,

        SiteCreated = 5,
        SiteDeleted = 6,
        SiteUpdated = 7,

        LockCreated = 8,
        LockDeleted = 9,
        LockUpdated = 10,
        LockAccessListUpdated = 11,

        CardholderCreated = 12,
        CardholderDeleted = 13,
        CardholderUpdated = 14,

        ScheduleCreated = 15,
        ScheduleDeleted = 16,
        ScheduleUpdated = 17,
    }
}
