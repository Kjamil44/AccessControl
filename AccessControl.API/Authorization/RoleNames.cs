namespace AccessControl.API.Authorization
{
    public class RoleNames
    {
        public const string SystemAdmin = "SystemAdmin";
        public const string SiteAdmin = "SiteAdmin";
        public const string SecurityOperator = "SecurityOperator";
        public const string CardholderManager = "CardholderManager";
        public const string Auditor = "Auditor";

        public static readonly string[] AllRoles =
        {
            "SystemAdmin",
            "SiteAdmin",
            "SecurityOperator",
            "CardholderManager",
            "Auditor"
        };
    }
}
