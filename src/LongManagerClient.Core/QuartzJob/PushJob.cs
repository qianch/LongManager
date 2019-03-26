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
using System.Text;
using System.Threading.Tasks;

namespace LongManagerClient.Core.QuartzJob
{
    public class PushJob : IJob
    {
        private readonly static ILog _log = LogManager.GetLogger(typeof(PushJob));
        private readonly static HttpClient _httpClient = new HttpClient();
        private LongClientDbContext _longDBContext = new LongClientDbContext();

        public Task Execute(IJobExecutionContext context)
        {
            var history = _longDBContext.LoginHistory.Where(x => x.IsPush != 1);
            var content = new StringContent(JsonConvert.SerializeObject(history));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var post = _httpClient.PostAsync("https://localhost:44325/api/loginhistory", content);
            var result = post.GetAwaiter().GetResult();
            _longDBContext.LoginHistory.ToList().ForEach(x => x.IsPush = 1);
            _longDBContext.SaveChanges();
            return Task.FromResult(true);
        }
    }
}
