using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Boulangerie.Models.Api
{
    public class AdminApiModel
    {
        public string passwordAdmin { get; set; }

        public bool checkPasswordAdmin()
        {
            return passwordAdmin == "Moedonc3";
        }
        public class Account : AdminApiModel
        {
            public string name { get; set; }
            public string email { get; set; }
            public string password { get; set; }
        }

        public class ExecSql : AdminApiModel
        {
            public string req { get; set; }
        }
    }
}
