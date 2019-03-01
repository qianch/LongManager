using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.DataBase
{
    [Table("MailBasicInfo")]
    public class MailBasicInfo : BaseEntity
    {
        public string MailNO { get; set; }
        public string Address { get; set; }
    }
}
