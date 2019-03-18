using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    [Table("LabelBasicInfo")]
    public class LabelBasicInfo : BaseEntity
    {
        public string LabelNO { get; set; }
    }
}
