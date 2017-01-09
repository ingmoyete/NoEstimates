using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.ClientValidations.Attributes
{
    public class IsNiceInteger : ValidationAttribute
    {
        public int NiceInteger { get; set; }

        public IsNiceInteger()
        {
            ErrorMessage = "{0} is not a nice integer.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            bool isNiceInteger = (value != null && NiceInteger == (int)value);
            return isNiceInteger ? ValidationResult.Success : new ValidationResult(string.Format(ErrorMessage, validationContext.MemberName));
        }
    }
}
