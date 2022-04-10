using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public interface IOrderServices
    {
        Task<Data.DbResponse<IEnumerable<Order>>> Search(OrderServices.SearchArgument criteria);
        Task<Data.DbResponse<Order>> Get(int ID_Commande);
        Task<Data.DbResponse<Order>> Save(Order commande);
        Task<Data.DbResponse<Order>> Delete(Order commande);
        Task<Data.DbResponse<Order.StatusOrder>> UpdateStatut(Order commande, Order.StatusOrder newStatut);
    }
}
