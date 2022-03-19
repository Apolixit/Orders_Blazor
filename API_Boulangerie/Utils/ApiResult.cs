using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Orders.Utils
{
    public class ApiResult
    {
        public bool IsOk { get; set; }
        public string ErrorMessage { get; set; }

        public static ApiResult Ok()
        {
            return new ApiResult()
            {
                IsOk = true,
                ErrorMessage = string.Empty
            };
        }

        public static ApiResult Error(string errorMessage_)
        {
            return new ApiResult()
            {
                IsOk = false,
                ErrorMessage = errorMessage_
            };
        }

        public static void ThrowDbError(Data.DbState state, string element)
        {
            if(state != Data.DbState.OK)
            {
                string errorMessage = string.Empty;
                switch(state)
                {
                    case Data.DbState.INVALID_INPUT:
                        errorMessage = $"Erreur avant d'accès à la BDD. L'élément {element} passé en paramètre n'est pas valide.";
                        break;
                    case Data.DbState.NOT_FOUND:
                        errorMessage = $"L'élément {element} n'a pas été trouvé dans la base de donnée.";
                        break;
                    case Data.DbState.ERROR:
                        errorMessage = $"Erreur de la BDD. Un exception à été catch pour l'élément {element}.";
                        break;
                    throw new Exception(errorMessage);
                }
            }
        }
    }
}
