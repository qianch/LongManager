using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManager.Core.DataBase
{
    [Table("CarBasicInfo")]
    public class CarBasicInfo : BaseEntity
    {
        public string CarNO { get; set; }
    }
}
