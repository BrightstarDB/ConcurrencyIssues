using BrightstarDB.Client;
using BrightstarDB.Storage;

namespace BrightstarDB.ConcurrencyIssue
{
    public class Startup
    {
        public static string ConnectionString = @"type=embedded;storesdirectory=C:\bstar\;storename=kp_data";
        public static string StoreName = "kp_data";

        public static void CreateStore()
        {
            // not sure why I used two different approaches between scripts to check if store exists....


            // from database seed dispatch script
            var client = BrightstarService
                .GetClient(ConnectionString);

            if (client.DoesStoreExist(StoreName))
                return;
            else
            {
                // execute all user db seed scripts

                // from database seed user script
                IDataObjectContext doc = BrightstarService.GetDataObjectContext(ConnectionString);

                if (doc.DoesStoreExist(StoreName))
                    return;

                doc.CreateStore(StoreName, persistenceType: PersistenceType.Rewrite);
                //doc.CreateStore(StoreName, persistenceType: PersistenceType.AppendOnly);

                // ... init db with user defined stuff ...

                using (var ctx = new Context1(ConnectionString))
                {
                    var e2 = ctx.ComplexEntity2s.Create();
                    e2.Name = "default";

                    var e1 = ctx.ComplexEntity1s.Create();
                    e1.Counter = 1000;

                    ctx.SaveChanges();
                }
            }
        }
    }
}