using API_Boulangerie;
using API_Boulangerie.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Boulangerie.Models.Api
{
    /// <summary>
    /// Modèle utilisé par l'api CommandesController
    /// </summary>
    public class CommandesApiModel
    {
        private static ServicesFactory _services => new ServicesFactory();

        public int id_commande { get; set; }
        public ClientApiModel client { get; set; }
        public DateTime dateLivraison { get; set; }
        public Commande.Statut statut { get; set; }
        public bool paye { get; set; }
        public string commentaire { get; set; }
        public IEnumerable<QuantityItem> produits { get; set; }
        public string produitDetailsCustom { get; set; }

        #region Mapping avec la base
        public static CommandesApiModel _FromBase(API_Boulangerie.Commande commande)
        {
            if (commande == null) return null;

            return new CommandesApiModel()
            {
                commentaire = commande.commentaire,
                dateLivraison = commande.dtCommande,
                paye = commande.isPaye,
                statut = commande.statut,
                id_commande = commande.ID_Commande,
                client = ClientApiModel._FromBase(commande.client),
                produitDetailsCustom = commande.autreDetails,
                produits = null //commande.details?.Select(x => QuantityItem._FromBase(x)).ToList()
            };
        }

        public API_Boulangerie.Commande _ToBase()
        {
            if (produits == null) return null;
            var commande = new API_Boulangerie.Commande()
            {
                ID_Commande = this.id_commande,
                ID_Client = this.client?.id_client,
                client = this.client._ToBase(),
                dtCommande = this.dateLivraison,
                isPaye = this.paye,
                statut = this.statut,
                typePaiement = API_Boulangerie.Commande.TypePaiement.ESPECES,
                commentaire = this.commentaire,
                autreDetails = this.produitDetailsCustom,
                details = null
            };
            commande.details = produits.Where(x => x.quantite > 0).Select(x => x._ToBase(commande)).ToList();
            return commande;
        }
        #endregion

        public static CommandesApiModel Get(int id)
        {
            return _FromBase(_services.CommandesServices.Get(id).data);
        }

        public static IEnumerable<CommandesApiModel> GetByDate(DateTime dtBegin, DateTime dtEnd)
        {
            IEnumerable<CommandesApiModel> commandesModel = null;
            IEnumerable<API_Boulangerie.Commande> commandesBase = _services.CommandesServices.Search(new CommandeServices.SearchArgument()
            {
                dtCommande = new API_Boulangerie.Utils.DateRange()
                {
                    dtDebut = dtBegin,
                    dtFin = dtEnd
                }
            }).data;
            if (commandesBase != null)
            {
                commandesModel = commandesBase.Select(x => _FromBase(x));
            }
            return commandesModel;
        }

        //public void Save()
        //{
        //    _services.CommandesServices.Save(_ToBase());
        //}

        //public void Delete()
        //{
        //    _services.CommandesServices.Delete(_ToBase());
        //}

        public class QuantityItem
        {
            private ServicesFactory _services => new ServicesFactory();

            public ProduitsApiModel produit { get; set; }
            public int quantite { get; set; }

            //public static QuantityItem _FromBase(API_Boulangerie.CommandeDetails commandeDetail)
            //{
            //    if (commandeDetail == null) return null;

            //    return new QuantityItem()
            //    {
            //        produit = ProduitsApiModel._FromBase(commandeDetail.produit),
            //        quantite = commandeDetail.quantite
            //    };
            //}

            public API_Boulangerie.CommandeDetails _ToBase(API_Boulangerie.Commande commande)
            {
                //produit = _services.ProduitServices.Get(this.produit.id),
                return new API_Boulangerie.CommandeDetails()
                {
                    commande = commande,
                    ID_Produit = this.produit.id_produit,
                    quantite = this.quantite
                };
            }
        }
    }
}
