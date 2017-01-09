using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoEstimates.Core.DataTableService
{
    public class DataTableJson<TDTO> where TDTO : class
    {
        public string sEcho { get; set; }
        public int iTotalRecords { get; set; }

        public int iTotalDisplayRecords { get; set; }

        public List<TDTO> aaData { get; set; }
    }
}
