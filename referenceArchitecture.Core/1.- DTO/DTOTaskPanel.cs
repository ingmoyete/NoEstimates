using NoEstimates.Core.Enums;
using referenceArchitecture.Core.Base.DTOBase;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DTO
{
    public class DTOTaskPanel : DTOBase
    {
        public DTOTaskPanel()
        {
            setDefaults();
        }

        public DTOTask Task { get; set; }
        public DTOHighlightColor Highlight { get; set; }
        public DTOComplete Complete { get; set; }
        public DTOTimer Timer { get; set; }

        private void setDefaults()
        {
            Highlight = new DTOHighlightColor
            {
                Color = (int)HighlightColor.Blue,
                Id = -1,
                TaskId = -1
            };

            Complete = new DTOComplete
            {
                IsComplete = false,
                Id = -1,
                TaskId = -1,
                FinalizationDate = (DateTime)SqlDateTime.MinValue
            };

            Timer = new DTOTimer
            {
                Id = -1,
                TaskId = -1,
                TimeInSeconds = 0
            };
        }
    }

    
}
