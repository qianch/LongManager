using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManager.Core.DataBase
{
    [Table("CarBasicInfo")]
    public class CarBasicInfo
    {
        [Key]
        public int ID { get; set; }
        public string RowGuid { get; set; }
        public string CarNO { get; set; }
    }
}
