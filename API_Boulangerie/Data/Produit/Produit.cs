using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Boulangerie
{
    public class Produit : Data.ICopy<Produit>, IProduitNode
    {
        [Key]
        public int ID_Produit { get; set; }
        [Required]
        public string Nom { get; set; }
        public int? ID_Part { get; set; }
        [ForeignKey("ID_Part")]
        public Part ProduitParts { get; set; }
        public string PathImage { get; set; }
        public int? ID_Categorie { get; set; }
        [ForeignKey("ID_Categorie")]
        public CategorieProduit categorie { get; set; }

        #region Interface tree
        [NotMapped]
        public int InternalID => ID_Produit;
        [NotMapped]
        public string ID_Node => $"Produit_{ID_Produit}";
        [NotMapped]
        public bool isLeaf => true;

        [NotMapped]
        public string Parent_ID
        {
            get
            {
                if (categorie == null) return null;
                else return categorie.ID_Node;
            }
        }
        #endregion

        public void Copy(Produit other)
        {
            this.Nom = other.Nom;
            this.PathImage = other.PathImage;
        }

        public bool Exist()
        {
            return this.ID_Produit > 0;
        }


    }
}
