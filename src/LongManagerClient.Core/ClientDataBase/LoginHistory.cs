using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class LoginHistory : BaseEntity
    {
        public string LoginUserName { get; set; }
        public string LoginDate { get; set; }
        public int IsPush { get; set; }
    }
}
