using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API_Boulangerie.Data;
using Microsoft.EntityFrameworkCore;

namespace API_Boulangerie.Services
{
    public class ProduitServices : IProduitServices
    {
        #region Produits
        /// <summary>
        /// Retour la liste complète de tout les produits
        /// </summary>
        /// <returns></returns>
        public Data.DbResponse<IEnumerable<Produit>> GetAll()
        {
            IEnumerable<Produit> produits = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    produits = db.Produits
                                .Include(x => x.categorie)
                                .ToList();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetAll] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Produit>>(produits, _status);
        }

        /// <summary>
        /// Récupère un produit par son ID
        /// </summary>
        /// <param name="ID_Produit"></param>
        /// <returns></returns>
        public Data.DbResponse<Produit> Get(int ID_Produit)
        {
            Produit produit = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    produit = db.Produits.FirstOrDefault(x => x.ID_Produit == ID_Produit);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Get] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Produit>(produit, _status);
        }

        /// <summary>
        /// Recherche des produits selon des critères de recherche
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public Data.DbResponse<IEnumerable<Produit>> Search(SearchArgument criteria)
        {
            IEnumerable<Produit> produits = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    produits = db.Produits.Where(x => String.IsNullOrEmpty(criteria.nom) || x.Nom.Contains(criteria.nom)).ToList();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Search] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Produit>>(produits, _status);
        }

        /// <summary>
        /// Sauvegarde un produit en base
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        public Data.DbResponse<Produit> Save(Produit produit)
        {
            if (produit == null) return new Data.DbResponse<Produit>(produit, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    if (!produit.Exist())
                    {
                        db.Produits.Add(produit);
                    }
                    else
                    {
                        var dbProduit = db.Produits.SingleOrDefault(x => x.ID_Produit == produit.ID_Produit);
                        if (dbProduit == null) return new Data.DbResponse<Produit>(produit, Data.DbState.NOT_FOUND);

                        dbProduit.Copy(produit);
                    }

                    if (produit.categorie != null)
                    {
                        db.Entry(produit.categorie).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                    }
                    if (produit.ProduitParts != null)
                    {
                        db.Entry(produit.ProduitParts).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Save] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Produit>(produit, _status);
        }

        /// <summary>
        /// Supprime un produit en base
        /// </summary>
        /// <param name="produit"></param>
        public Data.DbResponse<Produit> Delete(Produit produit)
        {
            if (produit == null) return new Data.DbResponse<Produit>(produit, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    db.Produits.Remove(produit);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Delete] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Produit>(produit, _status);
        }
        #endregion

        #region Catégorie de Produit
        /// <summary>
        /// Récupère une catégorie de produit via son ID
        /// </summary>
        /// <param name="ID_Cat"></param>
        /// <returns></returns>
        public Data.DbResponse<CategorieProduit> GetCat(int ID_Cat)
        {
            CategorieProduit cat = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    cat = db.CategorieProduit.FirstOrDefault(x => x.ID_Categorie == ID_Cat);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetCat] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<CategorieProduit>(cat, _status);
        }

        /// <summary>
        /// Sauvegarde une catégorie de produit
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public Data.DbResponse<CategorieProduit> SaveCat(CategorieProduit cat)
        {
            if (cat == null) return new Data.DbResponse<CategorieProduit>(cat, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    if (cat.Parent != null && !cat.Parent.Exist())
                    {
                        SaveCat(cat.Parent);
                    }

                    if (!cat.Exist())
                    {
                        db.CategorieProduit.Add(cat);
                    }
                    else
                    {
                        var dbCat = db.CategorieProduit.SingleOrDefault(x => x.ID_Categorie == cat.ID_Categorie);
                        if (dbCat == null) return new Data.DbResponse<CategorieProduit>(cat, Data.DbState.NOT_FOUND);

                        dbCat.Copy(cat);
                    }

                    if (cat.Parent != null)
                    {
                        db.Entry(cat.Parent).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                    }

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - SaveCat] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<CategorieProduit>(cat, _status);
        }

        /// <summary>
        /// Supprime une catégorie de produit
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public Data.DbResponse<CategorieProduit> DeleteCat(CategorieProduit cat)
        {
            if (cat == null) return new Data.DbResponse<CategorieProduit>(cat, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    db.CategorieProduit.Remove(cat);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - DeleteCat] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<CategorieProduit>(cat, _status);
        }
        #endregion

        #region Part de produit
        /// <summary>
        /// Retour la liste complète de toutes type de part
        /// </summary>
        /// <returns></returns>
        public DbResponse<IEnumerable<Part>> GetAllPart()
        {
            IEnumerable<Part> parts = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    parts = db.Part.ToList();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetAllPart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Part>>(parts, _status);
        }

        /// <summary>
        /// Récupère une type de part par son ID
        /// </summary>
        /// <param name="ID_Produit"></param>
        /// <returns></returns>
        public DbResponse<Part> GetPart(int ID_Part)
        {
            Part part = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    part = db.Part.FirstOrDefault(x => x.ID_Part == ID_Part);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetPart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Part>(part, _status);
        }

        /// <summary>
        /// Sauvegarde une part en base
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        public DbResponse<Part> SavePart(Part part)
        {
            if (part == null) return new Data.DbResponse<Part>(part, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    if (!part.Exist())
                    {
                        db.Part.Add(part);
                    }
                    else
                    {
                        var dbPart = db.Part.SingleOrDefault(x => x.ID_Part == part.ID_Part);
                        if (dbPart == null) return new Data.DbResponse<Part>(part, Data.DbState.NOT_FOUND);

                        dbPart.Copy(part);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - SavePart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Part>(part, _status);
        }

        /// <summary>
        /// Supprime une part en base
        /// </summary>
        /// <param name="produit"></param>
        public DbResponse<Part> DeletePart(Part part)
        {
            if (part == null) return new Data.DbResponse<Part>(part, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    db.Part.Remove(part);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - DeletePart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Part>(part, _status);
        }

        /// <summary>
        /// Retourne la liste des produits et leur catégorie sous forme de liste simple pour être transformé en Tree
        /// </summary>
        /// <returns></returns>
        public DbResponse<IEnumerable<IProduitNode>> GetFlatCategorieProduit()
        {
            List<IProduitNode> nodes = new List<IProduitNode>();
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BoulangerieContext())
                {
                    var produits = db.Produits
                                .Include(x => x.categorie)
                                .ToList();
                    var catProduits = db.CategorieProduit
                                .Include(x => x.Parent)
                                .ToList();

                    if(produits != null)
                        nodes.AddRange(produits.Cast<IProduitNode>());
                    if (catProduits != null)
                        nodes.AddRange(catProduits.Cast<IProduitNode>());
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetFlatCategorieProduit] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<IProduitNode>>(nodes, _status);
        }
        #endregion

        /// <summary>
        /// Classe de critère de recherche des produits
        /// </summary>
        public class SearchArgument
        {
            public string nom { get; set; }
        }
    }
}
