using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoEstimates.Core.DataTableService
{
    public interface IDataTableService
    {
        DataTableParams DataTableParams { get; }
        IQueryable<TDTO> getIQueryableSource<TDTO>(DataTableParams parameters,
                                                        IQueryable<TDTO> dtoModelInGrid,
                                                        Expression<Func<TDTO, bool>> filtering) where TDTO : class;
        DataTableJson<TDTO> getJsonResponse<TDTO>(List<TDTO> dtoModelInGrid) where TDTO : class;

        IQueryable<T> CustomOrderBy<T>(IQueryable<T> source) where T : class;

        DateTime getDateTimeField(string fieldValue, string dateFormat, string withSuffix = "", char withSeparator = char.MinValue);
    }
}
