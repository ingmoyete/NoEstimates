using referenceArchitecture.Core.Helpers;
using referenceArchitecture.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.Base.DTOBase
{
    public interface IDTOBase
    {
        IResource GlobalResources { get; set; }
        List<ValidationResult> ValidationResultCollection { get; set; }
        Ihp hp{ get; set;}
        List<object> getDTOPropertiesByNames(ValidationContext validationContext, params string[] propertyNames);
    }   
}
