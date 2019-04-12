using Newtonsoft.Json;
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
            using(SqlConnection con = new SqlConnection("Server=mti-dbs2.fgc.com;Initial Catalog=MEDW;User ID=scanner;Password=scanner"))
            {
                using (SqlCommand cmd = new SqlCommand("MTI.GET_BADGE_DATA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BadgeNo", SqlDbType.VarChar).Value = value.stringValue;
                    cmd.Parameters.Add("@isIndia", SqlDbType.Int).Value = 0;
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    con.Close();
                    return JsonConvert.SerializeObject(dt);
                }
            }
            
            return value.stringValue;
        }

        [HttpPost, Route("CheckPalletData")]
        public string CheckPalletData([FromBody] PalletDataModel value)
        {

            using (SqlConnection con = new SqlConnection("Server=mti-dbs2.fgc.com;Initial Catalog=MEDW;User ID=scanner;Password=scanner"))
            {
                using (SqlCommand cmd = new SqlCommand("MTI.GET_PALLET_SCAN", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@BadgeNo", SqlDbType.VarChar).Value = value.Badge_ID;
                    cmd.Parameters.Add("@PalletID", SqlDbType.VarChar).Value = value.Pallet_ID;
                    cmd.Parameters.Add("@ContID", SqlDbType.VarChar).Value = value.CONT_ID;
                    cmd.Parameters.Add("@isIndia", SqlDbType.Int).Value = value.isIndia;
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    con.Close();

                    return JsonConvert.SerializeObject(dt);
                    //var temp = JsonConvert.SerializeObject(dt);
                    //return JsonConvert.DeserializeObject<ReturnBadgeModel>(temp).EmpName;
                }
            }

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

        [HttpGet, Route("CheckAPIWorking")]
        public string CheckAPIWorking()
        {
            return "PalletBuild";
        }
    }
}
