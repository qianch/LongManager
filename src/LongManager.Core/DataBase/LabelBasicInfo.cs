using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManager.Core.DataBase
{
    [Table("LabelBasicInfo")]
    public class LabelBasicInfo : BaseEntity
    {
        public string LabelNO { get; set; }
    }
}
