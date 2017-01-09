using NoEstimates.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Helpers
{
    public interface Ihp
    {
        DTOLineFitParameters getRegressionParameters(double[] xData, double[] yData);
        string getLocalResourceKey();
        string getGlobalResourceKey();
        int getIntegerFromAppConfig(string key, Exception exception);
        int getIntegerFromAppConfig(string key);
        string getStringFromAppConfig(string key, Exception exception);
        string getStringFromAppConfig(string key);
        string getPathFromSeparatedCommaValue(string keyAppConfig);
    }
}
