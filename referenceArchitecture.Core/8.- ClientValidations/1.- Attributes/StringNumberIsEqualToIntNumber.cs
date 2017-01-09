using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.ClientValidations.Attributes
{
    class StringNumberIsEqualToIntNumber : ValidationAttribute
    {
        public string IntNumberPropertyToCompare { get; set; }
        public StringNumberIsEqualToIntNumber() { ErrorMessage = "{0} is not the same number that {1}"; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get property by name
            var dtoBase = validationContext.ObjectInstance as DTOBase;
            int numberToCompare = (int)dtoBase.getDTOPropertiesByNames(validationContext, IntNumberPropertyToCompare).FirstOrDefault();   
            
            // Perform validation
            bool isEqual = (value != null && numberToCompare.ToString() == value.ToString());

            return isEqual ? ValidationResult.Success : new ValidationResult(string.Format(ErrorMessage, IntNumberPropertyToCompare, validationContext.MemberName));
        }

    }
}
