namespace AccessControl.API.Authorization
{
    public class RoleNames
    {
        /// <summary>
        /// All Permissions
        /// </summary>
        public const string SystemAdmin = "SystemAdmin";

        /// <summary>
        /// Manages one or more Sites: users in that site, locks, cardholders, schedules, live events.
        /// </summary>
        public const string SiteAdmin = "SiteAdmin";

        /// <summary>
        /// Day-to-day operations and monitoring, not configuration.
        /// </summary>
        public const string SecurityOperator = "SecurityOperator";

        /// <summary>
        /// HR/Reception-style, manages people and their access windows, not locks.
        /// </summary>
        public const string CardholderManager = "CardholderManager";

        /// <summary>
        /// Read-only across the system for compliance & security review.
        /// </summary>
        public const string Auditor = "Auditor";

        public static readonly string[] AllRoles =
        {
            SystemAdmin,
            SiteAdmin,
            SecurityOperator,
            CardholderManager,
            Auditor
        };
    }
}
