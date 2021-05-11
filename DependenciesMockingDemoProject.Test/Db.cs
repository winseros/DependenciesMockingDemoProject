using System.Transactions;
using DependenciesMockingDemoProject.Web.DataLayer;
using DropSchema.SqlServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DependenciesMockingDemoProject.Test
{
    public static class Db
    {
        private const string AdmConnectionString =
            "Server=localhost;Database=DependenciesMockingDatabase;User Id=DependenciesMocking_Adm;Password=1QAZ2wsx3EDC;Connection Timeout=30;";

        internal const string AppConnectionString =
            "Server=localhost;Database=DependenciesMockingDatabase;User Id=DependenciesMocking_App;Password=1QAZ2wsx3EDC;Connection Timeout=30;";

        public static void Recreate()
        {
            using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew,
                new TransactionOptions {IsolationLevel = IsolationLevel.ReadCommitted}))
            {
                using var conn = new SqlConnection(AdmConnectionString);
                conn.Open();

                new DropOperation(conn).Run(SchemaConstants.SchemaName);

                scope.Complete();
            }

            var ctx = GetAdminContext();
            ctx.Database.Migrate();
        }

        public static WeatherDbContext GetAdminContext()
        {
            return new(Options.Create(new DataLayerOptions
                {ConnectionString = AdmConnectionString}), null);
        }

        public static WeatherDbContext GetAppContext()
        {
            return new(Options.Create(new DataLayerOptions
                {ConnectionString = AppConnectionString}), null);
        }
    }
}