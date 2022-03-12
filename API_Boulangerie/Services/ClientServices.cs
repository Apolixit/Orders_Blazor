using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Boulangerie.Services
{
    public class ClientServices : IClientServices
    {
        /// <summary>
        /// Récupère la liste complete des clients non supprimé
        /// </summary>
        /// <returns></returns>
        public Data.DbResponse<IEnumerable<Client>> GetAll()
        {
            Data.DbState _status = Data.DbState.OK;
            IEnumerable<Client> clients = null;
            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    clients = db.Clients
                                .Where(x => x.Active())
                                .ToList();
                }
            } catch(Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - GetAll()] Erreur lors de la récupération des données : {ex}");
            }
            return new Data.DbResponse<IEnumerable<Client>>(clients, _status);
        }

        /// <summary>
        /// Retourne le client depuis son ID
        /// </summary>
        /// <param name="ID_Client"></param>
        /// <returns></returns>
        public Data.DbResponse<Client> Get(int ID_Client)
        {
            Data.DbState _status = Data.DbState.OK;
            Client client = null;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    client = db.Clients
                                .Where(x => x.Active())
                                .SingleOrDefault(x => x.ID_Client == ID_Client);
                }
            } catch(Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Get(id)] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Recherche un client depuis les critères de recherche
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Data.DbResponse<IEnumerable<Client>> Search(SearchArgument criteria)
        {
            Data.DbState _status = Data.DbState.OK;
            IEnumerable<Client> lClient = null;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    lClient = db.Clients
                                .Where(x => (!string.IsNullOrEmpty(criteria.nom) && x.nomComplet.Contains(criteria.nom))
                                            || (!string.IsNullOrEmpty(criteria.telephone) && x.phoneNumber.Contains(criteria.telephone))
                                            || (!string.IsNullOrEmpty(criteria.email) && x.email.Contains(criteria.email)))
                                .ToList();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Search()] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Client>>(lClient, _status);
        }

        /// <summary>
        /// Sauvegarde un client
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Data.DbResponse<Client> Save(Client client)
        {
            if (client == null) return new Data.DbResponse<Client>(client, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
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
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Save(client)] Erreur lors de la récupération des données : {ex}");
            }
            
            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Supprime un client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Data.DbResponse<Client> Delete(Client client)
        {
            if (client == null) return new Data.DbResponse<Client>(client, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    db.Clients.Remove(client);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Save(client)] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Désactive le client (il ne sera plus retourné dans les recherches client, mais il continuera à être affecté aux anciennes commandes existantes pour éviter des incohérences)
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public Data.DbResponse<Client> Disable(Client client)
        {
            if (client == null) return new Data.DbResponse<Client>(null, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;
            
            try
            {
                using(var db = new Data.BoulangerieContext())
                {
                    var dbClient = db.Clients.SingleOrDefault(x => x.ID_Client == client.ID_Client);
                    if (dbClient == null) return new Data.DbResponse<Client>(client, Data.DbState.NOT_FOUND);
                    dbClient.disabled = true;
                    db.SaveChanges();
                }
            } catch(Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Disable] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Client>(client, _status);
        }

        /// <summary>
        /// Classe de critère de recherche des clients
        /// </summary>
        public class SearchArgument
        {
            public string nom { get; set; }
            public string telephone { get; set; }
            public string email { get; set; }
        }
    }
}
