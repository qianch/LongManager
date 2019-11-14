using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ServerDataBase
{
    [Table("T_EntryBill")]
    public class EntryBill
    {
        [Key, Display(Description = "邮件条码")]
        public string BarCode { get; set; }

        [Display(Description = "目的地")]
        public string DestAddress { get; set; }

        [Display(Description = "预分局")]
        public string PresortPost { get; set; }

        [Display(Description = "邮寄时间")]
        public DateTime? CreateDateTime { get; set; }
    }
}
