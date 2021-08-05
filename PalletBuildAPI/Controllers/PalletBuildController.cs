using Newtonsoft.Json;
using PalletBuildAPI.Models;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace PalletBuildAPI.Controllers
{
    [RoutePrefix("PalletBuild")]
    public class PalletBuildController : ApiController
    {
        //SQL Connection Config
        private string ServerName { get; set; }
        private string InitialCatalog { get; set; }
        private string Username { get; set; }
        private string Password { get; set; }

        //SQL Procedures Config
        private string Schema { get; set; }

        public PalletBuildController()
        {
            ServerName = ConfigurationManager.AppSettings["SQL.ServerName"];
            InitialCatalog = ConfigurationManager.AppSettings["SQL.InitialCatalog"];
            Username = ConfigurationManager.AppSettings["SQL.Username"];
            Password = ConfigurationManager.AppSettings["SQL.Password"];

            Schema = ConfigurationManager.AppSettings["SQL.Schema"];
        }

        [HttpPost, Route("CheckInUser")]
        public string CheckInUser([FromBody] StringModel value)
        {
            using (SqlConnection con = new SqlConnection($"Server={ServerName};Initial Catalog={InitialCatalog};User ID={Username};Password={Password}"))
            {
                using (SqlCommand cmd = new SqlCommand($"{Schema}.GET_BADGE_DATA", con))
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
            using (SqlConnection con = new SqlConnection($"Server={ServerName};Initial Catalog={InitialCatalog};User ID={Username};Password={Password}"))
            {
                using (SqlCommand cmd = new SqlCommand($"{Schema}.GET_PALLET_SCAN", con))
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
            using (SqlConnection con = new SqlConnection($"Server={ServerName};Initial Catalog={InitialCatalog};User ID={Username};Password={Password}"))
            {
                using (SqlCommand cmd = new SqlCommand($"{Schema}.GET_PALLET_COUNT", con))
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
            return  "PalletBuild API v:1.1";
        }
    }
}
