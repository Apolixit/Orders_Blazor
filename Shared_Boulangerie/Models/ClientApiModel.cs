using API_Boulangerie;
using API_Boulangerie.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Boulangerie.Models.Api
{
    public class ClientApiModel
    {
        private static ServicesFactory _services => new ServicesFactory();

        public int id_client { get; set; }
        public string nomComplet { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }

        #region Mapping avec la base
        public static ClientApiModel _FromBase(Client client)
        {
            if (client == null) return null;

            return new ClientApiModel()
            {
                id_client = client.ID_Client,
                nomComplet = client.nomComplet,
                phoneNumber = client.phoneNumber,
                email = client.email
            };
        }

        public Client _ToBase()
        {
            return new Client()
            {
                ID_Client = id_client,
                nomComplet = nomComplet,
                phoneNumber = phoneNumber,
                email = email
            };
        }
        #endregion

        public static ClientApiModel Get(int id)
        {
            return _FromBase(_services.ClientServices.Get(id).data);
        }

        public static List<ClientApiModel> GetAll()
        {
            return _services.ClientServices.GetAll().data.Select(x => _FromBase(x)).ToList();
        }

        public static List<ClientApiModel> Search(ClientServices.SearchArgument criteria)
        {
            return _services.ClientServices.Search(criteria).data.Select(x => _FromBase(x)).ToList();
        }
    }
}
