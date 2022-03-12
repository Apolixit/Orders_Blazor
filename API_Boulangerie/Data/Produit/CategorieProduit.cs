using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API_Boulangerie
{
    public class CategorieProduit : IProduitNode, Data.ICopy<CategorieProduit>
    {
        [Key]
        public int ID_Categorie { get; set; }
        [Required]
        public string Nom { get; set; }

        public int? ID_CategorieParent { get; set; }
        [ForeignKey("ID_CategorieParent")]
        public CategorieProduit Parent { get; set; }

        #region Interface tree
        [NotMapped]
        public int InternalID => ID_Categorie;

        [NotMapped]
        public string ID_Node => $"CategorieProduit_{ID_Categorie}";
        [NotMapped]
        public bool isLeaf => false;
        [NotMapped]
        public string Parent_ID
        {
            get
            {
                if (Parent == null) return null;
                else return Parent.ID_Node;
            }
        }
        #endregion


        public void Copy(CategorieProduit other)
        {
            this.Nom = other.Nom;
            this.Parent = other.Parent;
            this.ID_CategorieParent = other.Parent?.ID_Categorie;
        }

        public bool Exist()
        {
            return ID_Categorie > 0;
        }
    }
}
