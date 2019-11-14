using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ServerDataBase
{
    [Table("T_BillSegRoad_Export")]
    public class BillExport
    {
        [Key, Display(Description = "邮件条码")]
        public string BarCode { get; set; }

        [Display(Description = "目的地地址")]
        public string DestAddress { get; set; }

        //[Display(Description = "全国格口")]
        //public string CountryBinCode { get; set; }

        [Display(Description = "长三角格口")]
        public string BinCode { get; set; }

        [Display(Description = "寄达局")]
        public string CityName { get; set; }

        [Display(Description = "邮寄时间")]
        public DateTime? CreateDateTime { get; set; }
    }
}
