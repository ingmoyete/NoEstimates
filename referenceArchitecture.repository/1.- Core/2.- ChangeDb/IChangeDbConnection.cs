using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.Core.ChangeDb
{
    public interface IChangeDbConnection
    {
        void changeConnectionString(string newConnectionStringName, IDbContext context);
        void writeSqlLogs(IDbContext context);
        bool UseTestDB { get; }
        bool LogSqlQueries { get; }
    }
}
