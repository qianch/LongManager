using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    [Table("LoginHistory")]
    public class LoginHistory : BaseEntity
    {
        public string LoginDisplayName { get; set; }
        public string LoginUserName { get; set; }
        public string LoginDate { get; set; }
        public int IsPush { get; set; }
    }
}
