using referenceArchitecture.service.Core.Base.BaseController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.service.Core.Base.BaseController.DictionaryWrapper
{
    public class ModelStateDictionaryWrapper : IModelState
    {
        Dictionary<string, string> dictionary;

        public ModelStateDictionaryWrapper(Dictionary<string, string> _dictionary)
        {
            this.dictionary = _dictionary;
        }
        public bool IsValid
        {
            get
            {
                if (true)
                    return dictionary.Count == 0;
            }
        }

        public void AddModelError(string key, string value)
        {
            dictionary.Add(key, value);
        }
    }
}
