using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOProject : DTOBase
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime FinalizationDate { get; set; }

    }
}
