using PalletBuildAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace PalletBuildAPI.Controllers
{
    [RoutePrefix("PalletBuild")]
    public class PalletBuildController : ApiController
    {
        [HttpPost, Route("CheckInUser")]
        public string CheckInUser([FromBody] StringModel value)
        {
            //using (SqlConnection con = new SqlConnection("Source=mti-dbs2;Initial Catalog=MEDW;User ID=scanner;Password=scanner"))
            using (SqlConnection con = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;"))
            {
                using (SqlCommand cmd = new SqlCommand("MTI.GET_BADGE_DATA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Badgeno", SqlDbType.VarChar).Value = value.stringValue;

                    DataTable dataTable = new DataTable();

                    dataTable.Load(cmd.ExecuteReader());

                    con.Open();
                    cmd.ExecuteNonQuery();

                    var output = new StringBuilder();

                    var columnsWidths = new int[dataTable.Columns.Count];

                    // Get column widths
                    foreach (DataRow row in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            var length = row[i].ToString().Length;
                            if (columnsWidths[i] < length)
                                columnsWidths[i] = length;
                        }
                    }

                    // Get Column Titles
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        var length = dataTable.Columns[i].ColumnName.Length;
                        if (columnsWidths[i] < length)
                            columnsWidths[i] = length;
                    }

                    // Write Column titles
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        var text = dataTable.Columns[i].ColumnName;
                        output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
                    }
                    output.Append("|\n" + new string('=', output.Length) + "\n");

                    // Write Rows
                    foreach (DataRow row in dataTable.Rows)
                    {
                        for (int i = 0; i < dataTable.Columns.Count; i++)
                        {
                            var text = row[i].ToString();
                            output.Append("|" + PadCenter(text, columnsWidths[i] + 2));
                        }
                        output.Append("|\n");
                    }
                    return output.ToString();
                }
            }
            return "";
        }

        private static string PadCenter(string text, int maxLength)
        {
            int diff = maxLength - text.Length;
            return new string(' ', diff / 2) + text + new string(' ', (int)(diff / 2.0 + 0.5));

        }

        [HttpPost, Route("CheckPalletData")]
        public string CheckPalletData([FromBody] PalletDataModel value)
        {

            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    using (SqlCommand cmd = new SqlCommand("ProcedureName", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@Badge_ID", SqlDbType.VarChar).Value = value.Badge_ID;
            //        cmd.Parameters.Add("@Pallet_ID", SqlDbType.VarChar).Value = value.Pallet_ID;
            //        cmd.Parameters.Add("@Cont_ID", SqlDbType.VarChar).Value = value.CONT_ID;

            //        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            //        returnParameter.Direction = ParameterDirection.ReturnValue;

            //        con.Open();
            //        cmd.ExecuteNonQuery();

            //        return returnParameter;
            //    }
            //}
            return value.Badge_ID;
        }
    }
}
