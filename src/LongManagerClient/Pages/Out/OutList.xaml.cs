using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LongManagerClient.Pages.Out
{
    /// <summary>
    /// EMSList.xaml 的交互逻辑
    /// </summary>
    public partial class OutList : BasePage
    {
        public OutList()
        {
            InitializeComponent();

            Pager.PageIndexChange += Pager_PageIndexChange;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = _longDBContext.OutInfo.Count();
            Pager.InitButton();
            MailDataGrid.ItemsSource = _longDBContext.OutInfo
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            MailDataGrid.ItemsSource = _longDBContext.OutInfo
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var mails = _longDBContext.OutInfo.AsEnumerable();
            if (!string.IsNullOrEmpty(TxtMailNO.Text))
            {
                mails = mails.Where(x => x.MailNO.Contains(TxtMailNO.Text));
            }

            Pager.LongPage.AllCount = mails.Count();
            Pager.LongPage.Search = TxtMailNO.Text;
            Pager.InitButton();

            MailDataGrid.ItemsSource = mails
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void PositionBtn_Click(object sender, RoutedEventArgs e)
        {
            //格口已经划分好的地区
            var firstCity = _longDBContext.CityInfo.Where(x => x.Position != null).ToList();
            var mails = _longDBContext.OutInfo.Where(x => x.BelongOfficeName == null).ToList();
            foreach (var mail in mails)
            {
                foreach (var city in firstCity)
                {
                    if (mail.OrgName.Contains(city.CityName) || mail.OrgName.Contains(city.OfficeName))
                    {
                        mail.BelongOfficeName = city.CityName;
                        mail.Position = city.Position;
                        _longDBContext.OutInfo.Update(mail);
                    }
                }
            }

            mails = _longDBContext.OutInfo.Where(x => x.BelongOfficeName == null).ToList();
            foreach (var mail in mails)
            {
                var city = _longDBContext.CityInfo.Where(x => x.CityCode.Length == 6 && x.CityName.Contains(mail.OrgName)).FirstOrDefault();
                if (city != null)
                {
                    //上一级地区
                    var parentCityCode = city.CityCode.Substring(0, 4) + "00";
                    var parentCity = _longDBContext.CityInfo.Where(x => x.CityCode == parentCityCode).FirstOrDefault();
                    if (parentCity != null)
                    {
                        //上一级地区是否为格口划分的地区
                        if (parentCity.Position != null)
                        {
                            mail.BelongOfficeName = parentCity.OfficeName;
                            mail.Position = parentCity.Position;
                            _longDBContext.OutInfo.Update(mail);
                        }
                        //上一级地区是否有所属格口的划分
                        else if (parentCity.BelongCityCode != null)
                        {
                            var belongCity = firstCity.Where(x => x.CityCode == parentCity.BelongCityCode).FirstOrDefault();
                            if (belongCity != null)
                            {
                                mail.BelongOfficeName = belongCity.OfficeName;
                                mail.Position = belongCity.Position;
                                _longDBContext.OutInfo.Update(mail);
                            }
                        }
                    }
                }
            }

            _longDBContext.SaveChanges();
            MessageBox.Show("划分完成");
        }
    }
}
