using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Resources
{
    public interface IResource
    {
        string this[string key] { get;}
        Dictionary<string, string> getResources(string page);
        Dictionary<string, string> Resources { get; }

        string GlobalResourceFileName{ get; }
    }
}
