using AccessControl.API.Models;
using Marten;
using static AccessControl.API.Authorization.RoleNames;

namespace AccessControl.API.Authorization
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(IDocumentStore store)
        {
            //TODO
            using var session = store.OpenSession();

            // If roles already exist, don’t seed again
            var anyRole = await session.Query<Role>().AnyAsync();
            if (anyRole)
                return;

            var roles = new List<Role>
        {
            new Role
            {
                Name = SystemAdmin,
                IsSystemRole = true,
                Permissions = PermissionCatalog.AllPermissions.ToList()
            },
            new Role
            {
                Name = SiteAdmin,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    // Users
                    "user.create", "user.read", "user.update",

                    // Site
                    "site.read", "site.update", "site.assign_users", "site.assign_locks",

                    // Locks
                    "lock.create", "lock.read", "lock.update", "lock.delete",
                    "lock.lock", "lock.unlock", "lock.assign_schedule",

                    // Cardholders
                    "cardholder.create", "cardholder.read", "cardholder.update", "cardholder.delete",
                    "cardholder.assign_schedule",

                    // Schedules
                    "schedule.create", "schedule.read", "schedule.update", "schedule.delete",
                    "schedule.assign_to_lock", "schedule.assign_to_cardholder",

                    // Live events
                    "live_event.read", "live_event.acknowledge"
                }
            },
            new Role
            {
                Name = SecurityOperator,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    "site.read",
                    "lock.read", "lock.lock", "lock.unlock",
                    "cardholder.read",
                    "schedule.read",
                    "live_event.read", "live_event.acknowledge"
                }
            },
            new Role
            {
                Name = CardholderManager,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    "cardholder.create", "cardholder.read", "cardholder.update", "cardholder.delete",
                    "cardholder.assign_schedule",
                    "schedule.read",
                    "site.read"
                }
            },
            new Role
            {
                Name = Auditor,
                IsSystemRole = true,
                Permissions = new List<string>
                {
                    "live_event.export",
                    "live_event.read"
                }
            },
        };

            // Optional: validate against PermissionCatalog
            foreach (var role in roles)
            {
                if (role.Permissions.Any(p => !PermissionCatalog.IsValid(p)))
                {
                    throw new InvalidOperationException($"Role {role.Name} has invalid permission(s).");
                }

                session.Store(role);
            }

            await session.SaveChangesAsync();
        }
    }

}
