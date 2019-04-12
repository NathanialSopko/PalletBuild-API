using System.Collections.Generic;

namespace PalletBuildAPI.Models
{
    public class PalletDataModel
    {
        public string Badge_ID { get; set; }
        public string Pallet_ID { get; set; }
        public string CONT_ID { get; set; }
        public int isIndia { get; set; }
    }

    public class StringModel
    {
        public string stringValue { get; set; }
    }

    public class Over
    {
        public List<ReturnBadgeModel> list { get; set; }
    }

    public class ReturnBadgeModel
    {
        public int Column1 { get; set; }
        public string BadgeNo { get; set; }
        public string EmpName { get; set; }
    }



    //public class GetBadgeIDReturner
    //{
    //    public 
    //}

    public class GetPalletDataReturner
    {
        public bool success { get; set; }
        public string error_code { get; set; }
        public string error_text { get; set; }
    }
}