using referenceArchitecture.service.Core.Base.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace referenceArchitecture.service.Core.Base.BaseController.MVCWrapper
{
    public class ModelStateMVCWrapper : IModelState
    {
        ModelStateDictionary modelState;

        public ModelStateMVCWrapper(ModelStateDictionary _modelState)
        {
            this.modelState = _modelState;
        }
        public bool IsValid
        {
            get
            {
                return modelState.IsValid;
            }
        }

        public void AddModelError(string key, string value)
        {
            modelState.AddModelError(key, value);
        }
    }
}
