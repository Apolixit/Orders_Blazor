using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Orders;
using API_Orders.Services;
using API_Orders.Utils;
using Microsoft.AspNetCore.Mvc;
using Shared_Orders.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Orders.Controllers.api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientController : ApiController
    {
        // GET: api/v1/Client/Get
        [HttpGet]
        [Route("Get")]
        public ActionResult<ClientDTO> Get(int id)
        {
            if (id <= 0) return BadRequest();
            try
            {
                ClientDTO client = Client.Get(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ClientController - Get(id={id})] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }
        }

        // GET: api/v1/Client/Get
        [HttpGet]
        [Route("Gets")]
        public ActionResult<IEnumerable<ClientDTO>> Get()
        {
            try
            {
                IEnumerable<ClientDTO> client = Client.GetAll();
                return Ok(client);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ClientController - Gets()] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }
        }

        // GET: api/v1/Client/Search
        [HttpPost]
        [Route("Search")]
        public ActionResult<IEnumerable<ClientDTO>> Search([FromBody]ClientServices.SearchArgument criteria)
        {
            try
            {
                IEnumerable<ClientDTO> client = Client.Search(criteria);
                return Ok(client);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ClientController - Search()] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }
        }

        // POST  api/v1/Client/Save
        [HttpPost]
        [Route("Save")]
        public ApiResult Post([FromBody] ClientDTO model)
        {
            try
            {
                if (model == null) return ApiResult.Error(modelEmpty);
                return serviceCall(_services.Clients.Save(Client.ToBusinessObject(model)).statut);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ClientController - Post(model={model}] Erreur lors de l'appel au service : {ex}");
                return ApiResult.Error(internalError);
            }

        }

        //DELETE api/v1/Client/Delete
        [HttpDelete]
        [Route("Delete")]
        public ApiResult Delete(int id)
        {
            try
            {
                if (id <= 0) return ApiResult.Error(modelEmpty);

                var client = _services.Clients.Get(id).data;
                if (client == null) return ApiResult.Error(modelEmpty);

                return serviceCall(_services.Clients.Delete(client).statut);
            }
            catch (Exception ex)
            {
                Log.logger.Error($"[ClientController - Delete(id={id}] Erreur lors de l'appel au service : {ex}");
                return ApiResult.Error(internalError);
            }
        }
    }
}
