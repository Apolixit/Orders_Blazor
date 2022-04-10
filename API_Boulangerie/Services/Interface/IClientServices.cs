using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public interface IClientServices
    {
        /// <summary>
        /// Get the whole list of non deleted client
        /// </summary>
        /// <returns></returns>
        Task<Data.DbResponse<IEnumerable<Client>>> GetAll();

        /// <summary>
        /// Search a client with search criteria arguments
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        Task<Data.DbResponse<IEnumerable<Client>>> Search(ClientServices.SearchArgument criteria);

        /// <summary>
        /// Get the client from his ID
        /// </summary>
        /// <param name="ID_Client"></param>
        /// <returns></returns>
        Task<Data.DbResponse<Client>> Get(int ID_Client);

        /// <summary>
        /// Save a client
        /// It could be insertion or update
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<Data.DbResponse<Client>> Save(Client client);

        /// <summary>
        /// Delete a client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        Task<Data.DbResponse<Client>> Delete(Client client);
    }
}
