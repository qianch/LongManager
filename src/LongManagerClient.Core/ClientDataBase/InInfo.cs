using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class InInfo : BaseEntity
    {
        public string MailNO { get; set; }
        public string AddDate { get; set; }
        public string PostDate { get; set; }
        public string OrgName { get; set; }
        public string Consignee { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int IsPush { get; set; }
    }
}
