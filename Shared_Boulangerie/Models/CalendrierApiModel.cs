using API_Boulangerie.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Boulangerie.Models.Api
{
    /// <summary>
    /// Modèle utilisé par l'api CalendrierController
    /// </summary>
    public class CalendrierApiModel
    {
        private static ServicesFactory _services => new ServicesFactory();

        public DateTime dt { get; set; }
        public IEnumerable<SyntheseCommandeApiModel> synthese { get; set; }

        public CalendrierApiModel(DateTime dt_)
        {
            this.dt = dt_;
            synthese = _loadSynthese();
        }

        private IEnumerable<SyntheseCommandeApiModel> _loadSynthese()
        {
            IList<SyntheseCommandeApiModel> commandesModel = null;
            IEnumerable<API_Boulangerie.Commande> commandes = _services.CommandesServices.Search(new CommandeServices.SearchArgument()
            {
                dtCommande = new API_Boulangerie.Utils.DateRange(this.dt)
            }).data;

            if(commandes != null)
            {
                commandesModel = new List<SyntheseCommandeApiModel>();
                commandesModel.Add(new SyntheseCommandeApiModel()
                {
                    couleur = System.Drawing.Color.Red,
                    eTypeCommande = TypeCommande.UNITAIRE,
                    nbCommandes = commandes.Count()
                });

            }
            return commandesModel;
        }

        public static bool IsDateValid(DateTime dt)
        {
            return dt != DateTime.MinValue && dt != DateTime.MaxValue;
        }
    }

    public class SyntheseCommandeApiModel
    {
        public TypeCommande            eTypeCommande   { get; set; }
        public System.Drawing.Color    couleur         { get; set; }
        public int                     nbCommandes     { get; set; }
    }

    public enum TypeCommande
    {
        UNITAIRE,
        RECURRENT
    }
}
