using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Boulangerie
{
    public class CommandeDetails : Data.ICopy<CommandeDetails>
    {
        [Key]
        public int ID_CommandeDetails { get; set; }
        public int quantite { get; set; }

        public int ID_Commande { get; set; }
        [ForeignKey("ID_Commande")]
        public Commande commande { get; set; }

        public int ID_Produit { get; set; }
        [ForeignKey("ID_Produit")]
        public Produit produit { get; set; }

        public void Copy(CommandeDetails other)
        {
            this.ID_Produit = other.ID_Produit;
            this.quantite = other.quantite;
        }
    }
}
