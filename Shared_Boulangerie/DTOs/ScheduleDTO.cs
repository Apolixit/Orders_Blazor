using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Orders.DTO
{
    /// <summary>
    /// Modèle utilisé par l'api CalendrierController
    /// </summary>
    public class ScheduleDTO
    {
        public DateTime dt { get; set; }
        public IEnumerable<SummaryOrderDTO>? summary { get; set; }

        public ScheduleDTO() { }

        public ScheduleDTO(DateTime dt, IEnumerable<SummaryOrderDTO> synthese)
        {
            this.dt = dt;
            this.summary = synthese;
        }

        public static bool IsDateValid(DateTime dt)
        {
            return dt != DateTime.MinValue && dt != DateTime.MaxValue;
        }
    }

    public class SummaryOrderDTO
    {
        public OrderType                eOrderType   { get; set; }
        public System.Drawing.Color     color         { get; set; }
        public int                      nbOrders     { get; set; }
    }

    public enum OrderType
    {
        UNITARY,
        RECURRENT
    }
}
