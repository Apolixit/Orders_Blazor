using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Orders.DTO
{
    public class AdminDTO
    {
        public string? Password { get; set; }

        public bool checkPasswordAdmin()
        {
            return Password != null && Password == "Moedonc3";
        }
        public class Account : AdminDTO
        {
            public string? name { get; set; }
            public string? email { get; set; }
            public string? password { get; set; }
        }

        public class ExecSql : AdminDTO
        {
            public string? req { get; set; }
        }
    }
}
