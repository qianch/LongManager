using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManager.Core.DataBase
{
    [Table("FrameUser")]
    public class FrameUser
    {
        [Key]
        public int ID { get; set; }
        public string RowGuid { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
    }
}
