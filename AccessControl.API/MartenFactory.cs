using AccessControl.API.Models;
using Marten;
using Weasel.Core;

namespace AccessControl.API
{
    public static class MartenFactory
    {
        public static IDocumentSession Session { get; set; }    
        public static IDocumentStore CreateDocumentStore(string connectionString)
        {
            var store = DocumentStore.For(opts =>
            {
                opts.Connection(connectionString);

                opts.Schema.For<User>();

                opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            });
            return store;
        }
        public static IDocumentSession CreateDocumentSession(string connectionString)
        {
            var store = CreateDocumentStore(connectionString);
            var docTracking = DocumentTracking.IdentityOnly;
            //options.Tracking = DocumentTracking.IdentityOnly;
            //options.IsolationLevel = IsolationLevel.ReadCommitted;
            return store.OpenSession(docTracking);
        }
    }
}
