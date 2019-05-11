using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class CityInfo : BaseEntity
    {
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public string OfficeName { get; set; }
        public string BelongOfficeName { get; set; }
        public string BelongCityCode { get; set; }
        public string CountryPosition { get; set; }
        public string JiangSuPosition { get; set; }
    }
}
