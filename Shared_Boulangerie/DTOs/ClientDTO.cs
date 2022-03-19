using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Orders.DTO
{
    public class ClientDTO
    {
        public int id_client { get; set; }
        public string? fullName { get; set; }
        public string? phoneNumber { get; set; }
        public string? email { get; set; }
    }
}
