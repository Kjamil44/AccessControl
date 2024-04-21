using Marten;

namespace AccessControl.API
{
    public static class MartenFactory
    {
        public static IDocumentSession Session { get; set; }    
        public static IDocumentStore CreateDocumentStore()
        {
            var store = DocumentStore.For("User ID=postgres;Password=1234;Host=localhost;Port=6969;Database=AccessControlDb;");
            return store;
        }
        public static IDocumentSession CreateDocumentSession()
        {
            var store = CreateDocumentStore();
            var docTracking = DocumentTracking.IdentityOnly;
            //options.Tracking = DocumentTracking.IdentityOnly;
            //options.IsolationLevel = IsolationLevel.ReadCommitted;
            return store.OpenSession(docTracking);
        }
    }
}
