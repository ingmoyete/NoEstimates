using NoEstimates.repository._0.__Edmx;
using NoEstimates.repository.Core.ChangeDb;
using referenceArchitecture.repository.Edmx.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.repository.Edmx.BaseContextAndPartialClasses
{
    public class BaseContext : NoEstimatesDevelopmentEntities, IDbContext
    {
        //todo ARCHITECTURE create this context and inject the changedbconnection.
        public BaseContext(IChangeDbConnection _changeDbConnection)
        {
            _changeDbConnection.changeConnectionString("Database1Entities", this);
        }
    }
}
