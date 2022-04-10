using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders
{
    public class Log
    {
        public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static string ErrorFetchingData(string serviceName, string serviceMethod, Exception ex) => $"[{serviceName} - {serviceMethod}] Error while fetching data : {ex}";
    }
}
