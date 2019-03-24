using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerWeb.Core.WebDataBase
{
    [Table("FrameUser")]
    public class FrameUser : BaseEntity
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string DisplayName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
    }
}
