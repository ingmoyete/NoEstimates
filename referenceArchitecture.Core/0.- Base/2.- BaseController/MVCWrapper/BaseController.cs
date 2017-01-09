using referenceArchitecture.service.Core.Base.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.ModelBinding;
using referenceArchitecture.Core.Exceptions;

namespace referenceArchitecture.service.Core.Base.BaseController.MVCWrapper
{
    // Model state de MVC ==============
    public abstract class BaseController : Controller, IControllerUI
    {
        ModelStateMVCWrapper modelStateMVCWrapper;
        
        public IModelState ModelStateService
        {
            get
            {
                if (ModelState == null) throw new NullModelStateInMVCWrapper();

                if (modelStateMVCWrapper == null)
                {
                    modelStateMVCWrapper = new ModelStateMVCWrapper(ModelState);
                }

                return modelStateMVCWrapper;
            }

        }
    }

}
