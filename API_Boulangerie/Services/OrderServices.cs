﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace API_Orders.Services
{
    public class OrderServices : IOrderServices
    {
        private static OrderServices instance = null;

        public static OrderServices Instance
        {
            get
            {
                if (instance == null) instance = new OrderServices();
                return instance;
            }
        }

        //private void LogError(Exception ex) => Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex));

        private OrderServices() { }

        /// <summary>
        /// Retourne la commande depuis son id
        /// </summary>
        /// <param name="ID_Commande"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Order>> Get(int ID_Commande)
        {
            Order? commande = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    commande = await db.Orders
                                        .Include(c => c.Client)
                                        .Include(d => d.Details)
                                            .ThenInclude(p => p.product)
                                        .FirstOrDefaultAsync(x => x.ID_Order == ID_Commande);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex))
                //Log.logger.Error($"[CommandeServices - Get(id)] Error while fetching data : {ex}");
            }

            return new Data.DbResponse<Order>(commande, _status);
        }

        /// <summary>
        /// Recherche une commande en fonction des critères de recherche
        /// Si aucun critère => retourne tout
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<IEnumerable<Order>>> Search(SearchArgument criteria)
        {
            IEnumerable<Order>? results = null;
            Data.DbState _status = Data.DbState.OK;

            //Critère par défaut
            if (criteria == null) criteria = new SearchArgument();

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    results = await db.Orders
                                    .Include(c => c.Client)
                                    .Include(d => d.Details)
                                        //.ThenInclude(p => p.produit)
                                        //.Where(x => (criteria.dtCreation.IsEmpty() || criteria.dtCreation.IsInside(x.dtCreation)) && (criteria.dtCommande.IsEmpty() || criteria.dtCommande.IsInside(x.dtCommande)))
                                    .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex))
                //Log.logger.Error($"[CommandeServices - Search] Error while fetching data : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Order>>(results, _status); ;
        }

        /// <summary>
        /// Sauvegarde la commande
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Order>> Save(Order commande)
        {
            if (commande == null) return new Data.DbResponse<Order>(commande, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    if (!commande.Client.Exist())
                    {
                        db.Clients.Add(commande.Client);
                    } else
                    {
                        db.Entry(commande.Client).State = EntityState.Unchanged;
                    }

                    if (!commande.Exist())
                    {
                        
                        db.Orders.Add(commande);
                    }
                    else
                    {
                        var dbCommande = db.Orders
                                                .Include(d => d.Details)
                                                .SingleOrDefault(x => x.ID_Order == commande.ID_Order);
                        if (dbCommande == null) return new Data.DbResponse<Order>(commande, Data.DbState.NOT_FOUND);

                        dbCommande.Copy(commande);
                    }
                    
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex))
                //Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex));
            }
            
            return new Data.DbResponse<Order>(commande, _status);
        }

        /// <summary>
        /// Supprime la commande passée en paramètre
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Order>> Delete(Order commande)
        {
            if (commande == null) return new Data.DbResponse<Order>(commande, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    db.Orders.Remove(commande);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex))
                //Log.logger.Error($"[CommandeServices - Delete] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Order>(commande, _status);
        }

        /// <summary>
        /// Met à jour le statut de la commande
        /// </summary>
        /// <param name="commande"></param>
        /// <param name="newStatut"></param>
        /// <returns></returns>
        public async Task<Data.DbResponse<Order.StatusOrder>> UpdateStatut(Order commande, Order.StatusOrder newStatut)
        {
            if (commande == null) return new Data.DbResponse<Order.StatusOrder>(newStatut, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    var dbCommande = db.Orders.SingleOrDefault(x => x.ID_Order == commande.ID_Order);
                    if (dbCommande == null) return new Data.DbResponse<Order.StatusOrder>(newStatut, Data.DbState.NOT_FOUND);
                    dbCommande.Status = newStatut;

                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error(Log.ErrorFetchingData(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex))
                //Log.logger.Error($"[CommandeServices - UpdateStatut] Erreur lors de la récupération des données : {ex}");

            }

            return new Data.DbResponse<Order.StatusOrder>(newStatut, _status);
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
