using API_Orders.Data;
using API_Orders.Services;
using API_Orders.Utils;
using Microsoft.AspNetCore.Mvc;

namespace API_Orders.Controllers.api
{
    public class ApiController : ControllerBase
    {
        protected static ServicesFactory _services => ServicesFactory.Instance;
        protected const string modelEmpty = "Les données sont incorrects";
        protected const string internalError = "Une erreur est survenue";

        protected ApiResult serviceCall(DbState statut)
        {
            if (statut == DbState.OK)
            {
                return ApiResult.Ok();
            }
            else
            {
                return ApiResult.Error(string.Empty); //TODO mais pas ce soir j'ai la flemme
            }
        }
    }
}
