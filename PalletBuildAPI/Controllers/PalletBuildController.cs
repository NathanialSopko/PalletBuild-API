using Newtonsoft.Json;
using PalletBuildAPI.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace PalletBuildAPI.Controllers
{
    [RoutePrefix("PalletBuild")]
    public class PalletBuildController : ApiController
    {
        [HttpPost, Route("CheckInUser")]
        public string CheckInUser([FromBody] StringModel value)
        {
            using (SqlConnection con = new SqlConnection("Server=mti-dbs2.fgc.com;Initial Catalog=MEDW;User ID=scanner;Password=scanner"))
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

                    List<ReturnBadgeModel> model = JsonConvert.DeserializeObject<List<ReturnBadgeModel>>(JsonConvert.SerializeObject(dt));
                    model[0].EmpName = model[0].EmpName.Trim();

                    return JsonConvert.SerializeObject(model[0]);
                }
            }

            //return JsonConvert.SerializeObject(new ReturnBadgeModel { BadgeNo = "badge number", EmpName = "Sopko, Nate", Column1 = 0 });
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

                    List<GetPalletDataReturner> model = JsonConvert.DeserializeObject<List<GetPalletDataReturner>>(JsonConvert.SerializeObject(dt));
                    return JsonConvert.SerializeObject(model[0]);
                }
            }

            //return "";
        }

        [HttpPost, Route("GetTotalScanCount")]
        public string GetTotalScanCount([FromBody] PalletCountDataModel value)
        {
            using (SqlConnection con = new SqlConnection("Server=mti-dbs2.fgc.com;Initial Catalog=MEDW;User ID=scanner;Password=scanner"))
            {
                using (SqlCommand cmd = new SqlCommand("MTI.GET_PALLET_COUNT", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@PalletID", SqlDbType.VarChar).Value = value.Pallet_ID;
                    cmd.Parameters.Add("@isIndia", SqlDbType.Int).Value = value.isIndia;
                    con.Open();

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    con.Close();

                    List<GetPalletCountReturner> model = JsonConvert.DeserializeObject<List<GetPalletCountReturner>>(JsonConvert.SerializeObject(dt));
                    return JsonConvert.SerializeObject(model[0]);
                }
            }

            //return JsonConvert.SerializeObject(new GetPalletCountReturner { errno = "0", errtxt = "", totalScanned = 100 });
        }

        [HttpGet, Route("CheckAPIWorking")]
        public string CheckAPIWorking()
        {
            return "PalletBuild";
        }
    }
}
