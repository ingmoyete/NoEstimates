using referenceArchitecture.Core.Base.DTOBase;
using referenceArchitecture.Core.ClientValidations.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Core.DTO
{
    public class DTOExample : DTOBase, IValidatableObject  // <-- IMPORTANT !! Do not use IValidableOject if possible, since it get fired after the attribute is ok.
    {
        public int Identificator { get; set; }
        
        [StringNumberIsEqualToIntNumber(IntNumberPropertyToCompare = "SecondField", ErrorMessage = "This is a custom Error Message")] // <-- This is a custom attribute
        [StringLength(2)]
        [DisplayName("First Field (3 digit max)")]
        public string FirstField { get; set; }

        [IsNiceInteger(NiceInteger = 90)] // <-- This is a custom attribute
        [Required]
        [DisplayName("Second Field (Required)")]
        public int SecondField { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Third Field (DateTime)")]
        public DateTime ThirdField { get; set; }

        // IMPORTANT !! Do not use IValidableOject if possible, since it get fired after the attribute is ok.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new List<ValidationResult> { ValidationResult.Success, ValidationResult.Success};
        }


    }
}
