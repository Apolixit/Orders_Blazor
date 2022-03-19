using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Orders.Controllers.api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ConnectionController : ApiController
    {
        //GET:  api/v1/Connection/Connect
        //[HttpPost]
        //[Route("Connect")]
        //public ConnectionResponse Connect(ConnectionModel connection)
        //{
        //    ConnectionResponse result = null;
        //    if (connection == null || (connection.login != "Admin" && connection.password != "#Admin"))
        //    {
        //        result = ConnectionResponse.GetError("Les identifiants sont invalides");
        //    }
        //    else
        //    {
        //        result = ConnectionResponse.GetValid(Url.Action("Index", "Commandes"));
        //        HttpContext.Session.SetInt32("IsLogged", 1);
        //    }

        //    return result;
        //}
    }
}
