using AccessControl.API;
using AccessControl.API.Authorization;
using AccessControl.API.Models;
using Marten;
using static AccessControl.API.Authorization.PermissionCatalog;
using static AccessControl.API.Authorization.RoleNames;

public static class RoleSeeder
{
    public static async Task SeedAsync(string connectionString)
    {
        using var session = MartenFactory.CreateDocumentSession(connectionString);

        var anyRole = await session.Query<Role>().AnyAsync();
        if (anyRole)
            return;

        var roles = new List<Role>
        {
            // 1) SystemAdmin – everything
            new Role
            {
                Name = SystemAdmin,
                IsSystemRole = true,
                Permissions = AllPermissions.ToList()
            },

            // 2) SiteAdmin
            new Role
            {
                Name = SiteAdmin,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    // USER
                    PermissionCatalog.User.Create,
                    PermissionCatalog.User.Read,
                    PermissionCatalog.User.Update,

                    // SITE
                    PermissionCatalog.Site.Read,
                    PermissionCatalog.Site.Update,

                    // LOCK
                    PermissionCatalog.Lock.Create,
                    PermissionCatalog.Lock.Read,
                    PermissionCatalog.Lock.Update,
                    PermissionCatalog.Lock.Delete,
                    PermissionCatalog.Lock.LockAction,
                    PermissionCatalog.Lock.UnlockAction,
                    PermissionCatalog.Lock.AssignAllowedUser,

                    // ALLOWED USER
                    PermissionCatalog.AllowedUser.Assign,
                    PermissionCatalog.AllowedUser.Edit,
                    PermissionCatalog.AllowedUser.Remove,

                    // CARDHOLDER
                    PermissionCatalog.Cardholder.Create,
                    PermissionCatalog.Cardholder.Read,
                    PermissionCatalog.Cardholder.Update,
                    PermissionCatalog.Cardholder.Delete,

                    // SCHEDULE
                    PermissionCatalog.Schedule.Create,
                    PermissionCatalog.Schedule.Read,
                    PermissionCatalog.Schedule.Update,
                    PermissionCatalog.Schedule.Delete,

                    // LIVE EVENT
                    PermissionCatalog.LiveEvent.Read,
                    PermissionCatalog.LiveEvent.Export
                }
            },

            // 3) SecurityOperator
            new Role
            {
                Name = SecurityOperator,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    // SITE
                    PermissionCatalog.Site.Read,

                    // LOCK – operate, not configure
                    PermissionCatalog.Lock.Read,
                    PermissionCatalog.Lock.LockAction,
                    PermissionCatalog.Lock.UnlockAction,

                    // CARDHOLDER
                    PermissionCatalog.Cardholder.Read,

                    // SCHEDULE
                    PermissionCatalog.Schedule.Read,

                    // LIVE EVENT
                    PermissionCatalog.LiveEvent.Read,
                }
            },

            // 4) CardholderManager
            new Role
            {
                Name = CardholderManager,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    // SITE
                    PermissionCatalog.Site.Read,

                    // CARDHOLDER
                    PermissionCatalog.Cardholder.Create,
                    PermissionCatalog.Cardholder.Read,
                    PermissionCatalog.Cardholder.Update,
                    PermissionCatalog.Cardholder.Delete,

                    // SCHEDULE
                    PermissionCatalog.Schedule.Read,

                    // optional: if they manage who is allowed on locks
                    PermissionCatalog.AllowedUser.Assign,
                    PermissionCatalog.AllowedUser.Edit,
                    PermissionCatalog.AllowedUser.Remove
                }
            },

            // 5) Auditor – read-only
            new Role
            {
                Name = Auditor,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    // USER
                    PermissionCatalog.User.Read,

                    // SITE
                    PermissionCatalog.Site.Read,

                    // LOCK
                    PermissionCatalog.Lock.Read,

                    // CARDHOLDER
                    PermissionCatalog.Cardholder.Read,

                    // SCHEDULE
                    PermissionCatalog.Schedule.Read,

                    // LIVE EVENT
                    PermissionCatalog.LiveEvent.Read,
                    PermissionCatalog.LiveEvent.Export
                }
            }
        };

        foreach (var role in roles)
        {
            if (role.Permissions.Any(p => !PermissionCatalog.IsValid(p)))
            {
                throw new InvalidOperationException(
                    $"Role {role.Name} has invalid permission(s): " +
                    string.Join(", ", role.Permissions.Where(p => !PermissionCatalog.IsValid(p)))
                );
            }

            session.Store(role);
        }

        await session.SaveChangesAsync();
    }
}
