using referenceArchitecture.service.Core.Base.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.service.Core.Base.BaseController.DictionaryWrapper
{
    // Model state dictionary ===============
    public abstract class LikeBaseController : IControllerUI
    {
        ModelStateDictionaryWrapper modelStateDictionaryWrapper;

        public IModelState ModelStateService
        {
            get
            {
                if (modelStateDictionaryWrapper == null)
                {
                    modelStateDictionaryWrapper = new ModelStateDictionaryWrapper(new Dictionary<string, string>());
                }
                return modelStateDictionaryWrapper;
            }
        }
    }

}
