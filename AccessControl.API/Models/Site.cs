using Marten.Schema;

namespace AccessControl.API.Models
{
    public class Site
    {
        [ForeignKey(typeof(User))]
        public Guid UserId { get; set; }
        [Identity]
        public Guid SiteId { get; set; }
        public string DisplayName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Site(Guid userId, string displayName)
        {
            UserId = userId;
            DisplayName = displayName;
            DateCreated = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }
        public void UpdateSite(string displayName)
        {
            DisplayName = displayName;
            DateModified = DateTime.UtcNow;
        }
    }
}
