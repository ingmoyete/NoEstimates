using NoEstimates.Core.DataTableService;
using NoEstimates.Core.Helpers.DateHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NoEstimates.Core.DataTableService
{
    public class DataTableService : IDataTableService
    {
        /// <summary>
        /// Date helper
        /// </summary>
        IDateHp dateHp;

        /// <summary>
        /// Total records.
        /// </summary>
        int totalRecords;

        /// <summary>
        /// Total display
        /// </summary>
        int totalDisplay;

        /// <summary>
        /// DataTable parameters.
        /// </summary>
        DataTableParams dataTableParams;

        /// <summary>
        /// Constructor to inject dependencies.
        /// </summary>
        /// <param name="dateHp">Date helper dependency.</param>
        public DataTableService(IDateHp dateHp)
        {
            this.dateHp = dateHp;
        }

        /// <summary>
        /// Get an IQueryable source with the queries for (paging, sorting, filtering, and ordering).
        /// </summary>
        /// <typeparam name="TDTO">Type of the IQueryable source.</typeparam>
        /// <param name="parameters">Parameters of the datatable request.</param>
        /// <param name="dtoModelInGrid">IQueryable source which will be used to perform the queries.</param>
        /// <param name="filtering">Filtering to be applyied for the search.</param>
        /// <returns>An IQueryable source of type TDTO.</returns>
        public IQueryable<TDTO> getIQueryableSource<TDTO>(  DataTableParams parameters, 
                                                            IQueryable<TDTO> dtoModelInGrid,
                                                            Expression<Func<TDTO, bool>> filtering)  where TDTO : class
        {
            var numberOfRecords = dtoModelInGrid.Count();
            totalRecords = numberOfRecords;
            totalDisplay = numberOfRecords;
            this.dataTableParams = parameters;

            // Ordering
            // !! DO NOT FORGET TO INCLUDE THE NAME OF THE DTO PROPERTIES IN THE sName OF DATATABLESCRIPT
            dtoModelInGrid = CustomOrderBy<TDTO>(dtoModelInGrid);

            // Filtering
            if (!string.IsNullOrEmpty(dataTableParams.sSearch))
            {
                dtoModelInGrid = dtoModelInGrid.Where(filtering).AsQueryable();

                totalDisplay = dtoModelInGrid.Count();
            }

            // Paging
            dtoModelInGrid = dtoModelInGrid
                .Skip(dataTableParams.iDisplayStart)
                .Take(dataTableParams.iDisplayLength).AsQueryable();


            // Get list and return json
            return dtoModelInGrid;

        }

        /// <summary>
        /// Get the date from a string value with suffix and separator.
        /// </summary>
        /// <param name="fieldValue">Field value to convert into date.</param>
        /// <param name="dateFormat">Date format.</param>
        /// <param name="withSuffix">Suffix before the search value.</param>
        /// <param name="withSeparator">Separator between the suffix and the search value.</param>
        /// <returns>A datetime.</returns>
        public DateTime getDateTimeField(string fieldValue, string dateFormat, string withSuffix = "", char withSeparator = char.MinValue)
        {
            string valueToParse = string.Empty;
            // Get string value to parse with separator and suffix (if set in parameters)
            if (!string.IsNullOrEmpty(fieldValue) 
                && withSeparator != char.MinValue && withSuffix != null)
            {
                valueToParse =  fieldValue.Contains(withSuffix + withSeparator) 
                                ? fieldValue.Split(new char[] { withSeparator }, 2)[1]
                                : string.Empty;
            }
            // Get string value to parse
            else if (!string.IsNullOrEmpty(fieldValue))
            {
                valueToParse = fieldValue;
            }

            // True if fieldValue is a date. Otheriwse false
            DateTime dateSearch = dateHp.WholeMinDate;
            bool isDate = dateHp.isValidDate(valueToParse, dateFormat, out dateSearch);

            // Return date if it meets all the above criteria. Otherwise it returns the minDate.
            return isDate ? dateSearch : dateHp.WholeMinDate;
        }

        /// <summary>
        /// Get the json response for datatable as an object. It needs to be converted in json string.
        /// </summary>
        /// <typeparam name="TDTO">Type of the dto for the collection.</typeparam>
        /// <param name="dtoModelInGrid">IQueryable source to be converted into list.</param>
        /// <returns>A json response as a DataTableJson object.</returns>
        public DataTableJson<TDTO> getJsonResponse<TDTO>(List<TDTO> dtoModelInGrid) where TDTO : class
        {
            return new DataTableJson<TDTO>
            {
                sEcho = dataTableParams.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalDisplay,
                aaData = dtoModelInGrid
            };
        }

        /// <summary>
        /// Order a iqueryable source by ascending or descending.
        /// </summary>
        /// <typeparam name="T">Type of the source.</typeparam>
        /// <param name="source">Collection to be ordered.</param>
        /// <returns>An iqueryable object.</returns>
        public IQueryable<T> CustomOrderBy<T>(IQueryable<T> source) where T : class
        {
            // Get name of the property to order from datatableparams. 
            string ordering = dataTableParams.sColumns.Split(',')[dataTableParams.iSortCol_0];
            string ascOrDesc = dataTableParams.sSortDir_0;

            // Perform ordering and return iqueryable result
            var type = typeof(T);
            var property = type.GetProperty(ordering);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = ascOrDesc == "asc"
                ? Expression.Call(typeof(Queryable), "OrderBy", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp))
                : Expression.Call(typeof(Queryable), "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public DataTableParams DataTableParams { get { return dataTableParams; } }
    }
}
