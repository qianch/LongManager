using log4net;
using LongManagerClient.Core.ClientDataBase;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LongManagerClient.Core.QuartzJob
{
    [DisallowConcurrentExecution]
    public class LoginHistoryJob : BaseJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            var history = _longDBContext.LoginHistory.Where(x => x.IsPush != 1);
            if (history.Count() > 0 && IsNetWorkConnect() && _isRemot)
            {
                try
                {
                    var content = new StringContent(JsonConvert.SerializeObject(history));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var post = _httpClient.PostAsync("http://106.12.85.7/api/loginhistory", content);
                    var result = post.GetAwaiter().GetResult();
                    _longDBContext.LoginHistory.ToList().ForEach(x => x.IsPush = 1);
                    _longDBContext.SaveChanges();
                }
                catch (Exception e)
                {
                    _isRemot = false;
                    _log.Error(e.ToString());
                }
            }
            return Task.FromResult(true);
        }
    }
}
