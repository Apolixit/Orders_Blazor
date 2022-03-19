using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public interface IOrderServices
    {
        Data.DbResponse<IEnumerable<Order>> Search(OrderServices.SearchArgument criteria);
        Data.DbResponse<Order> Get(int ID_Commande);
        Data.DbResponse<Order> Save(Order commande);
        Data.DbResponse<Order> Delete(Order commande);
        Data.DbResponse<Order.StatusOrder> UpdateStatut(Order commande, Order.StatusOrder newStatut);
    }
}
