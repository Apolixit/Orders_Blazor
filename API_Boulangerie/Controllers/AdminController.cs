using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Orders;
using API_Orders.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared_Orders.DTO;

namespace API_Orders.Controllers.api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AdminController : ApiController
    {
        public UserManager<IdentityUser> userManager { get; }

        public AdminController(UserManager<IdentityUser> userManager_)
        {
            this.userManager = userManager_;
        }

        [HttpPost]
        [Route("ManageAccount")]
        public ApiResult ManageAccount(AdminDTO.Account account)
        {

            Log.logger.Info($"[AccountController - Create Account");
            try
            {
                if (!account.checkPasswordAdmin()) return ApiResult.Error("The admin password is not valid");

                IdentityUser admin = userManager.FindByNameAsync(account.name).Result;
                if(admin == null)
                {
                    admin = new IdentityUser()
                    {
                        Email = account.email,
                        UserName = account.name
                    };

                    //Account creation
                    var result = userManager.CreateAsync(admin, account.password).Result;
                }
                else
                {
                    if(!userManager.CheckPasswordAsync(admin, account.password).Result)
                    {
                        var resPassword = userManager.ResetPasswordAsync(admin, userManager.GeneratePasswordResetTokenAsync(admin).Result, account.password).Result;
                        if (!resPassword.Succeeded)
                            throw new Exception($"Password reinitialisation error for {account.name}");
                    }
                    var result = userManager.UpdateAsync(admin).Result;
                }
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[AccountController - CreateAdmin] Call error for FindByNameAsync : {ex}");
                return ApiResult.Error(ex.Message);
            }

            return ApiResult.Ok();
        }

        [HttpPost]
        [Route("ExecSQL")]
        public ApiResult ExecSQL(AdminDTO.ExecSql sql)
        {
            Log.logger.Info($"[AdminController - ExecSQL");
            try
            {
                if (!sql.checkPasswordAdmin()) return ApiResult.Error("Le mot de passe admin est incorrect");
                //_services.UtilsServices.ExecSQL(sql.req);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[AdminController - ExecSQL] Error request execution : {ex}");
                return ApiResult.Error(ex.Message);
            }
            return ApiResult.Ok();
        }
    }
}
