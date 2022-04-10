using API_Orders.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Orders.Services
{
    public class ClientServices : IClientServices
    {
        private static ClientServices instance = null;

        public static ClientServices Instance
        {
            get
            {
                if (instance == null) instance = new ClientServices();
                return instance;
            }
        }

        private ClientServices() { }


        /// <summary>
        /// Get the whole list of non deleted client
        /// </summary>
        /// <returns></returns>
        public async Task<Data.DbResponse<IEnumerable<Client>>> GetAll()
        {
            Data.DbState _status = Data.DbState.OK;
            IEnumerable<Client>? clients = null;
            try
            {
                using (var db = new Data.BakeryContext())
                {
                    clients = await db.Clients
                                .Where(x => !x.Disabled)
                                .ToListAsync();
                }
            } catch(Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - GetAll()] Error while fetching data : {ex}");
            }
            return new Data.DbResponse<IEnumerable<Client>>(clients, _status);
        }

        /// <summary>
        /// Get the client from his ID
        /// </summary>
        /// <param name="ID_Client"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Client>> Get(int ID_Client)
        {
            Data.DbState _status = Data.DbState.OK;
            Client? client = null;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    client = await db.Clients
                                .Where(x => !x.Disabled)
                                .SingleOrDefaultAsync(x => x.ID_Client == ID_Client);
                }
            } catch(Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Get(id)] Error while fetching data : {ex}");
            }

            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Recherche un client depuis les critères de recherche
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<IEnumerable<Client>>> Search(SearchArgument criteria)
        {
            Data.DbState _status = Data.DbState.OK;
            IEnumerable<Client>? lClient = null;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    var check = new CheckString();
                    lClient = await db.Clients
                                        .Where(x => check.Contains(x.FullName, criteria.nom)
                                                || check.Contains(x.PhoneNumber, criteria.telephone)
                                                || check.Contains(x.Email, criteria.email)
                                        )
                                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Search()] Error while fetching data : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Client>>(lClient, _status);
        }

        /// <summary>
        /// Save a client
        /// It could be insertion or update
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Client>> Save(Client client)
        {
            if (client == null) return new Data.DbResponse<Client>(client, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    if (!client.Exist())
                    {
                        db.Clients.Add(client);
                    }
                    else
                    {
                        var dbClient = db.Clients
                                        .SingleOrDefault(x => x.ID_Client == client.ID_Client);
                        if (dbClient == null) return new Data.DbResponse<Client>(client, Data.DbState.NOT_FOUND);

                        dbClient.Copy(client);
                    }
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Save(client)] Error while fetching data : {ex}");
            }
            
            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Delete a client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Client>> Delete(Client client)
        {
            if (client == null) return new Data.DbResponse<Client>(client, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    db.Clients.Remove(client);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Save(client)] Error while fetching data : {ex}");
            }

            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Désactive le client (il ne sera plus retourné dans les recherches client, mais il continuera à être affecté aux anciennes commandes existantes pour éviter des incohérences)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Client>> Disable(Client client)
        {
            if (client == null) return new Data.DbResponse<Client>(null, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;
            
            try
            {
                using(var db = new Data.BakeryContext())
                {
                    var dbClient = db.Clients.SingleOrDefault(x => x.ID_Client == client.ID_Client);
                    if (dbClient == null) return new Data.DbResponse<Client>(client, Data.DbState.NOT_FOUND);

                    dbClient.Disabled = true;
                    await db.SaveChangesAsync();
                }
            } catch(Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Disable] Error while fetching data : {ex}");
            }

            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Classe de critère de recherche des clients
        /// </summary>
        public class SearchArgument
        {
            public string? nom { get; set; }
            public string? telephone { get; set; }
            public string? email { get; set; }
        }
    }
}
