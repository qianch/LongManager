using CefSharp.ModelBinding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LongManagerClient.ModelBinding
{
    public class MethodInterceptorLogger : IMethodInterceptor
    {
        public object Intercept(Func<object> method, string methodName)
        {
            object result = method();
            Debug.WriteLine("Called " + methodName);
            return result;
        }
    }
}
