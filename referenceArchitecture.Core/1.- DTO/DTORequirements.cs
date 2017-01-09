using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTORequirements : DTOBase
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public bool IsComplete { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime FinalizationDate { get; set; }
    }
}
