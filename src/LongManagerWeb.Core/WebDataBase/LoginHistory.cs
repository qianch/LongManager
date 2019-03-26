using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerWeb.Core.WebDataBase
{
    [Table("LoginHistory")]
    public class LoginHistory : BaseEntity
    {
        public string LoginDisplayName { get; set; }
        public string LoginUserName { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
