using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Services
{
    public interface IClientServices
    {
        Data.DbResponse<IEnumerable<Client>> GetAll();
        Data.DbResponse<IEnumerable<Client>> Search(ClientServices.SearchArgument criteria);
        Data.DbResponse<Client> Get(int ID_Client);
        Data.DbResponse<Client> Save(Client client);
        Data.DbResponse<Client> Delete(Client client);
    }
}
