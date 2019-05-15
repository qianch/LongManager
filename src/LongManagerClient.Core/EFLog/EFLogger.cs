using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.EFLog
{
    public class EFLogger : ILogger
    {
        private readonly string categoryName;

        public EFLogger(string _categoryName) => categoryName = _categoryName;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (categoryName == "Microsoft.EntityFrameworkCore.Database.Command" && logLevel == LogLevel.Information)
            {
                var logContent = formatter(state, exception);
                Console.WriteLine(logContent);
            }
        }
    }
}
