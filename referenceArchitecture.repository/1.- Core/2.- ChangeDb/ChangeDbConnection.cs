using referenceArchitecture.Core.Helpers;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.Core.ChangeDb
{
    public class ChangeDbConnection : IChangeDbConnection
    {
        // Helper parameter
        private Ihp hp;

        /// <summary>
        /// Constructor used to inject the Ihp.
        /// </summary>
        /// <param name="_hp"></param>
        public ChangeDbConnection(Ihp _hp)
        {
            this.hp = _hp;
        }

        /// <summary>
        /// Change the connection string of the context.
        /// </summary>
        /// <param name="newConnectionStringName">New connection string to be set.</param>
        /// <param name="context">A context where the connection string will be changed.</param>
        public void changeConnectionString(string newConnectionStringName, IDbContext context)
        {
            context.Database.Connection.ConnectionString = GetConnectionString(newConnectionStringName, context);

            // Log the sql
            if (LogSqlQueries)
            {
                writeSqlLogs(context);
            }
        }

        /// <summary>
        /// Write sql query logs.
        /// </summary>
        /// <param name="context">Context of the database.</param>
        public void writeSqlLogs(IDbContext context)
        {
            context.Database.Log = message => Trace.Write(message);
        }

        /// <summary>
        /// Get a connection string in ado form from a entityFramework connection string.
        /// </summary>
        /// <param name="newConnectionStringName">EntityFramework connection string to be set.</param>
        /// <returns>An ado form connection string.</returns>
        private string GetConnectionString(string newConnectionStringName, IDbContext context)
        {
            if (!UseTestDB) return context.Database.Connection.ConnectionString;

            // Get ef connection string from app.config
            var EFConnectionString = ConfigurationManager.ConnectionStrings[newConnectionStringName].ConnectionString;

            // Convert the connectino string into a ado connection string
            var ADOConnectionString = new EntityConnectionStringBuilder(EFConnectionString);
            return ADOConnectionString.ProviderConnectionString;
        }

        /// <summary>
        /// If true, it uses the connection string of test. If false it uses the connection string of development.
        /// </summary>
        public bool UseTestDB { get { return hp.getStringFromAppConfig("useTestDB") == "true"; } }

        /// <summary>
        /// If true, the sql queries are 
        /// </summary>
        public bool LogSqlQueries { get { return hp.getStringFromAppConfig("logSql") == "true"; } }

    }
}
