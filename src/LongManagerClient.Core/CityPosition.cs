using LongManagerClient.Core.ClientDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LongManagerClient.Core
{
    public class CityPosition
    {
        private LongClientDbContext _dbContext;
        public readonly List<CityInfo> _countryPositionCity = new List<CityInfo>();
        public readonly List<CityInfo> _jiangsuPositionCity = new List<CityInfo>();

        public CityPosition(LongClientDbContext dbContext)
        {
            _dbContext = dbContext;
            //全国格口划分好的区域
            _countryPositionCity = dbContext.CityInfo.Where(x => !string.IsNullOrEmpty(x.CountryPosition)).ToList();
            //长三角格口划分好的区域
            _jiangsuPositionCity = dbContext.CityInfo.Where(x => !string.IsNullOrEmpty(x.JiangSuPosition)).ToList();
        }
        /// <summary>
        /// 根据CityInfo中配置的格口对应关系直接查找
        /// </summary>
        /// <param name="mail"></param>
        public void CountryPositionByCityCode(BaseOut mail)
        {
            foreach (var city in _countryPositionCity)
            {
                var officeName = city.OfficeName ?? city.CityName;
                var names = city.CityName + officeName + city.AliasName;
                if (names.Contains(mail.OrgName) ||
                    mail.OrgName.Contains(city.CityName) ||
                    mail.OrgName.Contains(officeName))
                {
                    mail.BelongOfficeName = city.CityName;
                    mail.CountryPosition = city.CountryPosition;
                }
            }
        }

        /// <summary>
        /// 先查找出CityInfo中上一级地区的CityCode,根据这个CityCode查找
        /// </summary>
        /// <param name="mail"></param>
        public void CountryPositionByParentCityCode(BaseOut mail)
        {
            var city = _dbContext.CityInfo.Where(x => x.CityCode.Length == 6 && (x.CityName + (string.IsNullOrEmpty(x.AliasName) ? "" : x.AliasName)).Contains(mail.OrgName)).FirstOrDefault();
            if (city != null)
            {
                //本级地区是否有属于全国格口的划分
                if (city.BelongCityCode != null)
                {
                    var belongCity = _countryPositionCity.Where(x => x.CityCode == city.BelongCityCode).FirstOrDefault();
                    if (belongCity != null)
                    {
                        mail.BelongOfficeName = belongCity.OfficeName;
                        mail.CountryPosition = belongCity.CountryPosition;
                        return;
                    }
                }

                //上一级地区
                var parentCityCode = city.CityCode.Substring(0, 4) + "00";
                var parentCity = _dbContext.CityInfo.Where(x => x.CityCode == parentCityCode).FirstOrDefault();
                if (parentCity != null)
                {
                    //上一级地区是否为全国格口划分的地区
                    if (parentCity.CountryPosition != null)
                    {
                        mail.BelongOfficeName = parentCity.OfficeName;
                        mail.CountryPosition = parentCity.CountryPosition;
                    }
                    //上一级地区是否有所属全国格口的划分
                    else if (parentCity.BelongCityCode != null)
                    {
                        var belongCity = _countryPositionCity.Where(x => x.CityCode == parentCity.BelongCityCode).FirstOrDefault();
                        if (belongCity != null)
                        {
                            mail.BelongOfficeName = belongCity.OfficeName;
                            mail.CountryPosition = belongCity.CountryPosition;
                        }
                    }
                }
            }
        }

        public void JiangSuPositionByCityCode(BaseOut mail)
        {
            foreach (var city in _jiangsuPositionCity)
            {
                if (mail.BelongOfficeName.Contains(city.OfficeName??city.CityName))
                {
                    mail.JiangSuPosition = city.JiangSuPosition;
                }
            }
        }
    }
}
