using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOTask : DTOBase
    {
        public int Id { get; set; }

        public int RequirementId { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

    }
}
