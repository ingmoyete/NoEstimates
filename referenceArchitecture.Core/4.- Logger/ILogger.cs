using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Logger
{
    public interface ILogger
    {
        void writeLog(Exception ex);

        void writeLog(string message);
    }
}
