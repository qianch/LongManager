using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class BaseOut : BaseEntity
    {
        public string MailNO { get; set; }
        public DateTime AddDate { get; set; }
        public string PostDate { get; set; }
        public string OrgName { get; set; }
        public string Consignee { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int IsPush { get; set; }
        public string BelongOfficeName { get; set; }
        public string CountryPosition { get; set; }
        public string JiangSuPosition { get; set; }
    }
}
