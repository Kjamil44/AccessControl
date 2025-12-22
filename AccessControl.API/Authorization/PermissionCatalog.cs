namespace AccessControl.API.Authorization
{
    public static class PermissionCatalog
    {
        public static class User
        {
            public const string Create = "user.create";
            public const string Read = "user.read";
            public const string Update = "user.update";
            public const string Delete = "user.delete";
        }

        public static class Site
        {
            public const string Create = "site.create";
            public const string Read = "site.read";
            public const string Update = "site.update";
            public const string Delete = "site.delete";
        }

        public static class Lock
        {
            public const string Create = "lock.create";
            public const string Read = "lock.read";
            public const string Update = "lock.update";
            public const string Delete = "lock.delete";

            public const string LockAction = "lock.lock";
            public const string UnlockAction = "lock.unlock";

            public const string AssignAllowedUser = "lock.assign_allowed_user";
        }

        public static class AllowedUser
        {
            public const string Assign = "allowed_user.assign";
            public const string Edit = "allowed_user.edit";
            public const string Remove = "allowed_user.remove";
        }

        public static class Cardholder
        {
            public const string Create = "cardholder.create";
            public const string Read = "cardholder.read";
            public const string Update = "cardholder.update";
            public const string Delete = "cardholder.delete";
        }

        public static class Schedule
        {
            public const string Create = "schedule.create";
            public const string Read = "schedule.read";
            public const string Update = "schedule.update";
            public const string Delete = "schedule.delete";
        }

        public static class LiveEvent
        {
            public const string Read = "live_event.read";
            public const string Export = "live_event.export";
        }

        public static readonly string[] AllPermissions =
        {
            // USER
            User.Create, User.Read, User.Update, User.Delete,

            // SITE
            Site.Create, Site.Read, Site.Update, Site.Delete,

            // LOCK
            Lock.Create, Lock.Read, Lock.Update, Lock.Delete,
            Lock.LockAction, Lock.UnlockAction, Lock.AssignAllowedUser,

            // ALLOWED USER
            AllowedUser.Assign, AllowedUser.Edit, AllowedUser.Remove,

            // CARDHOLDER
            Cardholder.Create, Cardholder.Read, Cardholder.Update, Cardholder.Delete,

            // SCHEDULE
            Schedule.Create, Schedule.Read, Schedule.Update, Schedule.Delete,

            // LIVE EVENTS
            LiveEvent.Read, LiveEvent.Export
        };

        public static bool IsValid(string permission)
            => AllPermissions.Contains(permission);
    }
}
