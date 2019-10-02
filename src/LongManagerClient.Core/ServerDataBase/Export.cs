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
        [Key]
        public string BarCode { get; set; }
        public string DestAddress { get; set; }
        public string BinCode { get; set; }
    }
}
