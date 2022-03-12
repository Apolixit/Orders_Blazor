using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Boulangerie.Services;
using API_Boulangerie.Utils.Tree;

namespace Shared_Boulangerie.Models.Api
{
    /// <summary>
    /// Modèle utilisé par l'api ProduitsController
    /// </summary>
    public class ProduitsApiModel
    {
        private static ServicesFactory _services => new ServicesFactory();

        public int id_produit { get; set; }
        public string nom { get; set; }
        public PartApiModel part { get; set; }
        public CategorieProduitApiModel categorie { get; set; }

        #region Mapping avec la base

        //public static ProduitsApiModel _FromBase(Produit produit)
        //{
        //    if (produit == null) return null;
        //    return new ProduitsApiModel()
        //    {
        //        id_produit = produit.ID_Produit,
        //        nom = produit.Nom,
        //        categorie = CategorieProduitApiModel._FromBase(produit.categorie),
        //        part = PartApiModel._FromBase(produit.ProduitParts)
        //    };
        //}

        //public Produit _ToBase()
        //{
        //    return new Produit()
        //    {
        //        ID_Produit = id_produit,
        //        Nom = nom,
        //        categorie = categorie._ToBase(),
        //        ProduitParts = part._ToBase()
        //    };
        //}
        #endregion

        //public static IEnumerable<ProduitsApiModel> GetAll()
        //{
        //    return _services.ProduitServices.GetAll().data.Select(x => ProduitsApiModel._FromBase(x));
        //}

        //public static ProduitsApiModel Get(int id)
        //{
        //    return ProduitsApiModel._FromBase(_services.ProduitServices.Get(id).data);
        //}

        //public static IEnumerable<TreeProduit> AsTree()
        //{
        //    var lProduits = _services.ProduitServices.GetFlatCategorieProduit().data;
        //    return TreeProduit.Build(TreeItem<IProduitNode>.GetTree(lProduits, null));
        //}
    }
}
