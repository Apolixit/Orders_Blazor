using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Boulangerie
{
    public class Commande : Data.ICopy<Commande>, IEntitySaveChange
    {
        [Key]
        public int ID_Commande { get; set; }
        public int? ID_Client { get; set; }
        [ForeignKey("ID_Client")]
        public Client client { get; set; }
        //Date à laquelle la commande a été passée
        public DateTime dtCreation { get; set; }
        public DateTime dtLastUpdated { get; set; }
        //Date à laquelle la commande doit être effectuée
        public DateTime dtCommande { get; set; }
        public Statut statut { get; set; }
        public TypePaiement typePaiement { get; set; }
        public bool isPaye { get; set; }
        public string commentaire { get; set; }
        public IEnumerable<CommandeDetails> details { get; set; }
        public string autreDetails { get; set; }

        public enum Statut
        {
            EN_COURS,
            TERMINEE,
            ANNULEE,
            EN_ERREUR
        }

        public enum TypePaiement
        {
            UNSET,
            ESPECES,
            CARTE_BANCAIRE,
            CHEQUE,
            AVOIR,
            AUTRE
        }

        public bool Exist()
        {
            return this.ID_Commande > 0;
        }

        public void Copy(Commande other)
        {
            this.client = other.client;
            this.dtCommande = other.dtCommande;
            this.statut = other.statut;
            this.isPaye = other.isPaye;
            this.typePaiement = other.typePaiement;
            this.commentaire = other.commentaire;
            this.autreDetails = other.autreDetails;
            if(this.details != null && other.details != null)
            {
                this.details = other.details.Select(x => x).ToList();
            }
        }

        public void onInsert()
        {
            this.dtCreation = DateTime.Now;
            this.dtLastUpdated = DateTime.Now;
        }

        public void onUpdate()
        {
            this.dtLastUpdated = DateTime.Now;
        }
    }
}
