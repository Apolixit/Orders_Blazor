using API_Boulangerie;
using API_Boulangerie.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Boulangerie.Models.Api
{
    public class CategorieProduitApiModel
    {
        private static ServicesFactory _services => new ServicesFactory();

        public int id_categorie { get; set; }
        public string nom { get; set; }
        public CategorieProduitApiModel cat { get; set; }

        #region Mapping avec la base

        public static CategorieProduitApiModel _FromBase(CategorieProduit cat)
        {
            if (cat == null) return null;
            return new CategorieProduitApiModel()
            {
                id_categorie = cat.ID_Categorie ,
                nom = cat.Nom,
                cat = _FromBase(cat?.Parent)
            };
        }

        public CategorieProduit _ToBase()
        {
            //var cat = _services.ProduitServices.GetCat(ID_Categorie);

            return new CategorieProduit()
            {
                ID_Categorie = this.id_categorie,
                Nom = this.nom,
                Parent = this.cat?._ToBase()
            };
        }
        #endregion

        //public void Save()
        //{
        //    _services.ProduitServices.SaveCat(_ToBase());
        //}

        //public void Delete()
        //{
        //    _services.ProduitServices.DeleteCat(_ToBase());
        //}
    }
}
