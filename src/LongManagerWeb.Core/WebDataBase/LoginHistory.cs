using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerWeb.Core.WebDataBase
{
    [Table("LoginHistory")]
    public class LoginHistory : BaseEntity
    {
        public string LoginDisplayName { get; set; }
        public string LoginUserName { get; set; }

        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd HH:mm:ss}")]
        public DateTime LoginDate { get; set; }
    }
}
