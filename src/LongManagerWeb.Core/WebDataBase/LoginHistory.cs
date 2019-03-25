using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerWeb.Core.WebDataBase
{
    public class LoginHistory : BaseEntity
    {
        public string LoginUserName { get; set; }
        public string LoginIP { get; set; }
        public DateTime LoginDate { get; set; }
    }
}
