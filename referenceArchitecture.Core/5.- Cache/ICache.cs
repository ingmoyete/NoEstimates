using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Cache
{
    public interface ICache
    {
        void remove(string key);

        Object get(String key);

        void setWithAbsoluteExpiration(string key, object data, string filePathDependency = null, int? minutesExpiration = null);

        void setWithSlideExpiration(string key, object data, string filePathDependency = null, int? minutesExpiration = null);

        int SlidingExpiration { get; }

        int AbsoluteExpiration { get; }

    }
}
