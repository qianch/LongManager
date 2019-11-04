using log4net;
using LongManagerClient.Core.ClientDataBase;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LongManagerClient.Core.CEF.JSObject
{
    public class CallbackObjectForJs
    {
        protected readonly static ILog _log = LogManager.GetLogger(typeof(CallbackObjectForJs));
        private LongClientDbContext _longDBContext = new LongClientDbContext();
        public void showMsg(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void saveOutInfo(string detailInfo)
        {
            var details = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(detailInfo);
            foreach (var detail in details)
            {
                var mailNO = detail.ContainsKey("waybillNo") ? detail["waybillNo"].ToString() : "";
                var receiverArriveOrgName = detail.ContainsKey("receiverArriveOrgName") ? detail["receiverArriveOrgName"].ToString() : "";
                var receiverAddr = detail.ContainsKey("receiverAddr") ? detail["receiverAddr"].ToString() : "";
                var receiverLinker = detail.ContainsKey("receiverLinker") ? detail["receiverLinker"].ToString() : "";
                var bizOccurDate = detail.ContainsKey("bizOccurDate") ? detail["bizOccurDate"].ToString() : DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var count = _longDBContext.OutInfo.Where(x => x.MailNO == mailNO).ToList().Count();
                if (count == 0)
                {
                    var mail = new OutInfo
                    {
                        RowGuid = Guid.NewGuid().ToString(),
                        MailNO = mailNO,
                        Address = receiverAddr,
                        OrgName = receiverArriveOrgName,
                        Consignee = receiverLinker,
                        AddDate = DateTime.Now,
                        PostDate = bizOccurDate
                    };
                    _longDBContext.OutInfo.Add(mail);
                    _longDBContext.SaveChanges();
                }
            }
        }

        public void saveInAddress(string mailNO, string address, string orgName, string consignee)
        {
            var count = _longDBContext.InInfo.Where(x => x.MailNO == mailNO).ToList().Count();
            if (count == 0)
            {
                var mail = new InInfo
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    MailNO = mailNO,
                    Address = address,
                    OrgName = orgName,
                    Consignee = "",
                    AddDate = DateTime.Now
                };
                _longDBContext.InInfo.Add(mail);
                _longDBContext.SaveChanges();
            }
        }
    }
}
