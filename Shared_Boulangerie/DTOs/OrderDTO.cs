using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Orders.DTO
{
    /// <summary>
    /// Modèle utilisé par l'api CommandesController
    /// </summary>
    public class OrderDTO
    {
        public int id_order { get; set; }
        public ClientDTO client { get; set; }
        public DateTime deliveryDate { get; set; }
        public StatusCommandeDTO status { get; set; }
        public bool isAlreadyPaid { get; set; }
        public string commentary { get; set; }
        public IEnumerable<QuantityItem> products { get; set; }
        public string customDetails { get; set; }

        public enum StatusCommandeDTO
        {
            RUNNING,
            FINISHED,
            CANCEL,
            ERROR
        }



        //public void Save()
        //{
        //    _services.CommandesServices.Save(_ToBase());
        //}

        //public void Delete()
        //{
        //    _services.CommandesServices.Delete(_ToBase());
        //}

        public class QuantityItem
        {
            public ProductDTO? product { get; set; }
            public int quantity { get; set; }
        }
    }
}
