using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManager.Core.DataBase
{
    [Table("FrameConfig")]
    public class FrameConfig
    {
        [Key]
        public int ID { get; set; }
        public string RowGuid { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
    }
}
