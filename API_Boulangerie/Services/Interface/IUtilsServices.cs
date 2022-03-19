using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public interface IUtilsServices
    {
        Data.DbResponse<string> ExecSQL(string req);
    }
}
