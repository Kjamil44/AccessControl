using AccessControl.API.Handlers.SiteHandlers;
using Marten.Schema;
using System.Security.Policy;

namespace AccessControl.API.Models
{
    public class Site
    {
        [Identity]
        public Guid SiteId { get; set; } 
        public string DisplayName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Site(string displayName)
        {
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
