using log4net;
using LongManagerClient.Core.ClientDataBase;
using Quartz;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LongManagerClient.Core.QuartzJob
{
    [DisallowConcurrentExecution]
    public abstract class BaseJob : IJob
    {
        protected readonly static ILog _log = LogManager.GetLogger(typeof(LoginHistoryJob));
        protected readonly static HttpClient _httpClient = new HttpClient();
        protected LongClientDbContext _longDBContext = new LongClientDbContext();
        protected static bool _isRemot = true;

        [DllImport("wininet")]
        protected extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        public static bool IsNetWorkConnect()
        {
            return InternetGetConnectedState(out int i, 0) ? true : false;
        }

        public abstract Task Execute(IJobExecutionContext context);
    }
}
