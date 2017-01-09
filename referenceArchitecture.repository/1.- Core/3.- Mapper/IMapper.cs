using NoEstimates.Core.DTO;
using NoEstimates.repository._0.__Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoEstimates.repository.Core.Mapper
{
    public interface IMapper
    {
        Project mapProjectEntity(DTOProject dtoProject);
        DTOProject mapProjectDTO(Project entityProject);
        DTORequirements mapDTORequirements(Requirement entityRequirement);
        Requirement mapEntityRequirements(DTORequirements dtoRequirement);
        DTOTask mapDTOTask(Task entityTask);
        Task mapEntityTask(DTOTask dtoTask);
        DTOHighlightColor mapDTOHighlight(Highlight entityHighlight);
        Highlight mapEntityHighlight(DTOHighlightColor DTOHighlight);
        DTOComplete mapDTOComplete(Complete Complete);
        Complete mapEntityComplete(DTOComplete Complete);
        DTOTimer mapDTOTimer(Timer timer);
        Timer mapEntityTimer(DTOTimer timer);
    }
}
