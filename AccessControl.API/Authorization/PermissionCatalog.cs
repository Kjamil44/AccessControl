namespace AccessControl.API.Authorization
{
    public class PermissionCatalog
    {
        public static readonly string[] AllPermissions =
        {
            "user.create",
            "user.read",
            "user.update",
            "user.delete",

            "site.create",
            "site.read",
            "site.update",
            "site.delete",

            "lock.create",
            "lock.read",
            "lock.update",
            "lock.delete",
            "lock.lock",
            "lock.unlock",

            "allowed_user.assign",
            "allowed_user.edit",
            "allowed_user.remove",

            "cardholder.create",
            "cardholder.read",
            "cardholder.update",
            "cardholder.delete",

            "schedule.create",
            "schedule.read",
            "schedule.update",
            "schedule.delete",

            "live_event.read",
            "live_event.acknowledge",
            "live_event.export",
        };
    }
}
