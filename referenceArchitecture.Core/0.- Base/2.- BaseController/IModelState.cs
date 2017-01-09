using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.service.Core.Base.BaseController
{
    public interface IModelState
    {
        void AddModelError(string key, string value);
        bool IsValid { get; }
    }
}
