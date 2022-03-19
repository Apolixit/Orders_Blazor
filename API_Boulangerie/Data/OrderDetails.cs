using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Orders
{
    public class OrderDetails : Data.ICopy<OrderDetails>
    {
        [Key]
        public int ID_OrderDetails { get; set; }
        public int quantity { get; set; }

        public int ID_Order { get; set; }
        [ForeignKey("ID_Order")]
        public Order order { get; set; }

        public int ID_Product { get; set; }
        [ForeignKey("ID_Product")]
        public Product product { get; set; }

        public void Copy(OrderDetails other)
        {
            this.ID_Product = other.ID_Product;
            this.quantity = other.quantity;
        }
    }
}
