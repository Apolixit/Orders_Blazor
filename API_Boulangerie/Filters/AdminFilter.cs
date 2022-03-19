using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Orders.Filters
{
    public class AdminFilter : ActionFilterAttribute
    {
        public string Password { get; set; }
        public AdminFilter(string password)
        {
            Password = password;
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Password = "TEST";
            
            base.OnResultExecuting(context);
        }
    }
}
