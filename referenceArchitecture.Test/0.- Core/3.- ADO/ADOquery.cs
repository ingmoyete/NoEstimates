using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace referenceArchitecture.Test.Core.ADO
{
    public class ADOquery
    {
        private string connectionString;
        public ADOquery()
        {
            connectionString = "Data Source=camellia.arvixe.com;Initial Catalog=ExampleDB;Persist Security Info=True;User ID=example;Password=example;MultipleActiveResultSets=True;Application Name=EntityFramework";
        }

        public long elapsedTimeADOquery(string query, params SqlParameter[] parameters)
        {
            Stopwatch stopwatch = new Stopwatch();

            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, con))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }

                stopwatch.Start();
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
                stopwatch.Stop();
            }

            return stopwatch.ElapsedMilliseconds;
        }
    }
}
