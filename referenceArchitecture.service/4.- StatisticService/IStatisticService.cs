using NoEstimates.Core.DTO;
using NoEstimates.service.Core.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.service.StatisticService
{
    public interface IStatisticService : IBaseService
    {
        DTOStatisticView getRequirementStatistic(DTOStatisticView statistic);
        DTOStatisticView getProjectStatistic(DTOStatisticView statistic);
    }
}
