using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    [Table("FrameConfig")]
    public class FrameConfig : BaseEntity
    {
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }
}
