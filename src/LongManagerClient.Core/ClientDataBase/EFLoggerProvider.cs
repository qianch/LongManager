﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class EFLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName) => new EFLogger(categoryName);

        public void Dispose()
        {
        }
    }
}
