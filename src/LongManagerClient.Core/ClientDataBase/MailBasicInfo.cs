﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    [Table("MailBasicInfo")]
    public class MailBasicInfo : BaseEntity
    {
        public string MailNO { get; set; }
        public string Consignee { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int IsPush { get; set; }
    }
}
