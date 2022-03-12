using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Boulangerie.Services
{
    public class CommandeServices : ICommandeServices
    {
        private static ServicesFactory _services => new ServicesFactory();

        /// <summary>
        /// Retourne la commande depuis son id
        /// </summary>
        /// <param name="ID_Commande"></param>
        /// <returns></returns>
        public Data.DbResponse<Commande> Get(int ID_Commande)
        {
            Commande commande = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    commande = db.Commandes
                                    .Include(c => c.client)
                                    .Include(d => d.details)
                                        .ThenInclude(p => p.produit)
                                    .FirstOrDefault(x => x.ID_Commande == ID_Commande);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[CommandeServices - Get(id)] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Commande>(commande, _status);
        }

        /// <summary>
        /// Recherche une commande en fonction des critères de recherche
        /// Si aucun critère => retourne tout
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Data.DbResponse<IEnumerable<Commande>> Search(SearchArgument criteria)
        {
            IEnumerable<Commande> results = null;
            Data.DbState _status = Data.DbState.OK;

            //Critère par défaut
            if (criteria == null) criteria = new SearchArgument();

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    results = db.Commandes
                                .Include(c => c.client)
                                .Include(d => d.details)
                                    .ThenInclude(p => p.produit)
                                    .Where(x => (criteria.dtCreation.IsEmpty() || criteria.dtCreation.IsInside(x.dtCreation)) && (criteria.dtCommande.IsEmpty() || criteria.dtCommande.IsInside(x.dtCommande)))
                                .ToList();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[CommandeServices - Search] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Commande>>(results, _status); ;
        }

        /// <summary>
        /// Sauvegarde la commande
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public Data.DbResponse<Commande> Save(Commande commande)
        {
            if (commande == null) return new Data.DbResponse<Commande>(commande, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    //if (commande.client.Exist())
                    //{
                    //    db.Clients.Update(commande.client);
                    //}
                    //else
                    //{
                    //    db.Clients.Add(commande.client);
                    //}

                    if (!commande.client.Exist())
                    {
                        db.Clients.Add(commande.client);
                    } else
                    {
                        db.Entry(commande.client).State = EntityState.Unchanged;
                    }

                    if (!commande.Exist())
                    {
                        
                        db.Commandes.Add(commande);
                    }
                    else
                    {
                        var dbCommande = db.Commandes
                                                .Include(d => d.details)
                                                .SingleOrDefault(x => x.ID_Commande == commande.ID_Commande);
                        if (dbCommande == null) return new Data.DbResponse<Commande>(commande, Data.DbState.NOT_FOUND);

                        dbCommande.Copy(commande);
                    }
                    //if(commande.client != null)
                    //{
                    //    db.Entry(commande.client)
                    //}
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[CommandeServices - Save] Erreur lors de la récupération des données : {ex}");
            }
            
            return new Data.DbResponse<Commande>(commande, _status);
        }

        /// <summary>
        /// Supprime la commande passée en paramètre
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public Data.DbResponse<Commande> Delete(Commande commande)
        {
            if (commande == null) return new Data.DbResponse<Commande>(commande, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    db.Commandes.Remove(commande);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[CommandeServices - Delete] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Commande>(commande, _status);
        }

        /// <summary>
        /// Met à jour le statut de la commande
        /// </summary>
        /// <param name="commande"></param>
        /// <param name="newStatut"></param>
        /// <returns></returns>
        public Data.DbResponse<Commande.Statut> UpdateStatut(Commande commande, Commande.Statut newStatut)
        {
            if (commande == null) return new Data.DbResponse<Commande.Statut>(newStatut, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    var dbCommande = db.Commandes.SingleOrDefault(x => x.ID_Commande == commande.ID_Commande);
                    if (dbCommande == null) return new Data.DbResponse<Commande.Statut>(newStatut, Data.DbState.NOT_FOUND);
                    dbCommande.statut = newStatut;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[CommandeServices - UpdateStatut] Erreur lors de la récupération des données : {ex}");

            }

            return new Data.DbResponse<Commande.Statut>(newStatut, _status);
        }

        /// <summary>
        /// Classe de critère de recherche des commandes
        /// </summary>
        public class SearchArgument
        {
            public Utils.DateRange dtCreation { get; set; }
            public Utils.DateRange dtCommande { get; set; }

            public SearchArgument()
            {
                this.dtCreation = new Utils.DateRange();
                this.dtCommande = new Utils.DateRange();
            }
        }
    }
}
