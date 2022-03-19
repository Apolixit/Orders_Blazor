using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Orders;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared_Orders.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_Orders.Controllers.api
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ScheduleController : ApiController
    {
        // GET api/v1/Calendrier/Get
        [HttpGet]
        [Route("Get")]
        public ActionResult<ScheduleDTO> Get(DateTime dt)
        {
            if (!ScheduleDTO.IsDateValid(dt)) return NotFound();

            try
            {
                ScheduleDTO model = new ScheduleDTO(dt, Order.LoadSummaryOrder(dt));
                return Ok(model);
            } catch(Exception ex)
            {
                Log.logger.Error($"[CalendrierController - Get(dt={dt})] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }
            
            
        }

        // GET api/v1/Commandes/GetRange
        [HttpGet]
        [Route("GetRange")]
        public ActionResult<IEnumerable<ScheduleDTO>> GetRange(DateTime dtBegin, DateTime dtEnd)
        {
            if (!ScheduleDTO.IsDateValid(dtBegin) || !ScheduleDTO.IsDateValid(dtEnd) && dtBegin < dtEnd) return NotFound();

            try
            {
                //Création de la liste des date de l'interval
                var lDate = Enumerable.Range(0, dtEnd.Subtract(dtBegin).Days + 1).Select(d => dtBegin.AddDays(d)).ToList();
                var lModel = lDate.Select(x => new ScheduleDTO(x, Order.LoadSummaryOrder(x)));

                return Ok(lModel);
            } catch(Exception ex)
            {
                Log.logger.Error($"[CalendrierController - GetRange(dtBegin={dtBegin}, dtEnd={dtEnd})] Erreur lors de l'appel au service : {ex}");
                return Forbid(ex.Message);
            }
            
        }
    }
}
