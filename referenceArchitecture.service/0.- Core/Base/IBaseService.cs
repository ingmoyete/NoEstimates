using referenceArchitecture.Core.Helpers;
using referenceArchitecture.Core.Resources;
using referenceArchitecture.repository.Edmx.Interfaces;
using referenceArchitecture.service.Core.Base.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.Core.Base
{
    public interface IBaseService
    {
        IControllerUI ControllerUI { get; }
        void register(IControllerUI _controllerUI);

        string SummaryError { get; }
        IResource GlobalResources { get; set; }

        IDbContext DbContext { get; set; }

        Ihp Hp { get; set; }

    }
}
