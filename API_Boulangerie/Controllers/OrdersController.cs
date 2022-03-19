using API_Orders;
using API_Orders.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared_Orders.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Orders.Controllers.api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : ApiController
    {
        //protected readonly IHubContext<Hubs.CommandeHub, Hubs.ICommandeHub> hubContext;

        //public OrdersController(IHubContext<Hubs.CommandeHub, Hubs.ICommandeHub> commandeHub)
        public OrdersController()
        {
            //this.hubContext = commandeHub;
        }

        // GET api/v1/Commandes/Get
        [HttpGet]
        [Route("Get")]
        public ActionResult<OrderDTO> Get(int id)
        {
            if (id <= 0) return Forbid();

            try
            {
                var commandeModel = Order.Get(id);
                return Ok(commandeModel);
            } catch(Exception ex)
            {
                Log.logger.Error($"[OrdersController - Get(id={id}] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }

            
        }

        // GET api/v1/Commandes/GetRange
        [HttpGet]
        [Route("GetRange")]
        public ActionResult<IEnumerable<OrderDTO>> GetRange(DateTime dtBegin, DateTime dtEnd)
        {
            if (!ScheduleDTO.IsDateValid(dtBegin) || !ScheduleDTO.IsDateValid(dtEnd) && dtBegin < dtEnd) return NotFound();
            try
            {
                IEnumerable<OrderDTO> lModel = Order.GetByDate(dtBegin, dtEnd);
                return Ok(lModel);
            } catch(Exception ex)
            {
                Log.logger.Error($"[OrdersController - GetRange(dtBegin={dtBegin}, dtEnd={dtEnd})] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }
            
        }

        // POST api/v1/Commandes/Save
        [HttpPost]
        [Route("Save")]
        public async Task<ApiResult> Post(OrderDTO model)
        {
            if (model == null) return ApiResult.Error(modelEmpty);
            try
            {
                var status = _services.Commandes.Save(Order.ToBusinessObject(model)).statut;

                //Hub SignalR
                //if(model.isEditing()) { await this.hubContext.Clients.All.CommandeChanged(model); }
                //else { await this.hubContext.Clients.All.CommandeCreated(model); }
               

                return serviceCall(status);
            } catch(Exception ex)
            {
                Log.logger.Error($"[OrdersController - Post(model={model}] Erreur lors de l'appel au service : {ex}");
                return ApiResult.Error(internalError);
            }
            
        }

        // GET api/v1/Commandes/UpdateStatut
        [HttpGet]
        [Route("UpdateStatut")]
        public ApiResult UpdateStatut(int id, Order.StatusOrder statut)
        {
            if (id <= 0) return ApiResult.Error(modelEmpty);
            try
            {
                var commandeBase = _services.Commandes.Get(id).data;
                if (commandeBase == null) return ApiResult.Error(modelEmpty);

                return serviceCall(_services.Commandes.UpdateStatut(commandeBase, statut).statut);
            } catch(Exception ex)
            {
                Log.logger.Error($"[OrdersController - UpdateStatut(id={id}, statut={statut}] Erreur lors de l'appel au service : {ex}");
                return ApiResult.Error(internalError);
            }
        }

        // DELETE api/v1/Commandes/Delete
        [HttpGet]
        [Route("Delete")]
        public async Task<ApiResult> Delete(int id)
        {
            if (id <= 0) return ApiResult.Error(modelEmpty);

            try
            {
                var commande = _services.Commandes.Get(id).data;
                if (commande == null) return ApiResult.Error(modelEmpty);

                //await this.hubContext.Clients.All.CommandeDeleted(CommandesDTO._FromBase(commande));

                return serviceCall(_services.Commandes.Delete(commande).statut);
            } catch(Exception ex)
            {
                Log.logger.Error($"[OrdersController - Delete(id={id}] Erreur lors de l'appel au service : {ex}");
                return ApiResult.Error(internalError);
            }
        }
    }
}
