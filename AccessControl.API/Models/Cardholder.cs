using AccessControl.API.Controllers;
using AccessControl.API.Handlers.CardholderHandlers;
using Baseline;
using Marten.Schema;

namespace AccessControl.API.Models
{
    public class Cardholder
    {
        [ForeignKey(typeof(Site))]
        public Guid SiteId { get; set; }
        [Identity]
        public Guid CardholderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName.Trim()} {LastName.Trim()}";
        public int CardNumber { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Cardholder(Guid siteId, string firstName, string lastName, int cardNumber)
        {
            SiteId = siteId;
            FirstName = firstName;
            LastName = lastName;
            CardNumber = cardNumber;
            ActivationDate = DateTime.UtcNow;
            ExpirationDate = DateTime.UtcNow.AddYears(1);
            DateCreated = DateTime.UtcNow;
            DateModified = DateTime.UtcNow;
        }
        public void UpdateCardholder(string firstName, string lastName, int cardNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            CardNumber = cardNumber;
            DateModified = DateTime.UtcNow;
        }
    }
}
