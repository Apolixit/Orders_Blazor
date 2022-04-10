using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using API_Orders.Data;
using API_Orders.Utils;
using Microsoft.EntityFrameworkCore;

namespace API_Orders.Services
{
    public class ProductServices : GetFlatProductCategory
    {
        private static ProductServices? instance = null;
        public static ProductServices Instance
        {
            get
            {
                if (instance == null) instance = new ProductServices();
                return instance;
            }
        }

        private ProductServices() { }


        #region Products
        /// <summary>
        /// Retour la liste complète de tout les produits
        /// </summary>
        /// <returns></returns>
        public async Task<DbResponse<IEnumerable<Product>>> GetAll()
        {
            IEnumerable<Product>? produits = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    produits = await db.Products
                                .Include(x => x.Category)
                                .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetAll] Erreur lors de la récupération des données : {ex}");
            }

            return new DbResponse<IEnumerable<Product>>(produits, _status);
        }

        /// <summary>
        /// Récupère un produit par son ID
        /// </summary>
        /// <param name="ID_Produit"></param>
        /// <returns></returns>
        public async Task<DbResponse<Product>> Get(int ID_Produit)
        {
            Product? produit = null;
            DbState _status = DbState.OK;

            try
            {
                using (var db = new BakeryContext())
                {
                    produit = await db.Products.FirstOrDefaultAsync(x => x.ID_Product == ID_Produit);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Get] Erreur lors de la récupération des données : {ex}");
            }

            return new DbResponse<Product>(produit, _status);
        }

        /// <summary>
        /// Recherche des produits selon des critères de recherche
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public async Task<DbResponse<IEnumerable<Product>>> Search(SearchArgument criteria)
        {
            IEnumerable<Product>? produits = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    var check = new CheckString();
                    produits = await db.Products
                                    .Where(x => check.Contains(x.Name, criteria.name))
                                    .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Search] Erreur lors de la récupération des données : {ex}");
            }

            return new DbResponse<IEnumerable<Product>>(produits, _status);
        }

        /// <summary>
        /// Sauvegarde un produit en base
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        public async Task<DbResponse<Product>> Save(Product produit)
        {
            if (produit == null) return new Data.DbResponse<Product>(produit, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    if (!produit.Exist())
                    {
                        db.Products.Add(produit);
                    }
                    else
                    {
                        var dbProduit = db.Products.SingleOrDefault(x => x.ID_Product == produit.ID_Product);
                        if (dbProduit == null) return new Data.DbResponse<Product>(produit, Data.DbState.NOT_FOUND);

                        dbProduit.Copy(produit);
                    }

                    if (produit.Category != null)
                    {
                        db.Entry(produit.Category).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                    }
                    if (produit.PortionProduct != null)
                    {
                        db.Entry(produit.PortionProduct).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                    }
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Save] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Product>(produit, _status);
        }

        /// <summary>
        /// Supprime un produit en base
        /// </summary>
        /// <param name="produit"></param>
        public async Task<DbResponse<Product>> Delete(Product produit)
        {
            if (produit == null) return new Data.DbResponse<Product>(produit, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    db.Products.Remove(produit);
                    await db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - Delete] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Product>(produit, _status);
        }
        #endregion

        #region Catégorie de Produit
        /// <summary>
        /// Récupère une catégorie de produit via son ID
        /// </summary>
        /// <param name="ID_Cat"></param>
        /// <returns></returns>
        public async Task<DbResponse<CategoryProduct>> GetCat(int ID_Cat)
        {
            CategoryProduct? cat = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    cat = await db.CategoryProduct.FirstOrDefaultAsync(x => x.ID_Category == ID_Cat);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetCat] Erreur lors de la récupération des données : {ex}");
            }

            return new DbResponse<CategoryProduct>(cat, _status);
        }

        /// <summary>
        /// Sauvegarde une catégorie de produit
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public async Task<DbResponse<CategoryProduct>> SaveCat(CategoryProduct cat)
        {
            if (cat == null) return new Data.DbResponse<CategoryProduct>(cat, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    if (cat.Parent != null && !cat.Parent.Exist())
                    {
                        SaveCat(cat.Parent);
                    }

                    if (!cat.Exist())
                    {
                        db.CategoryProduct.Add(cat);
                    }
                    else
                    {
                        var dbCat = db.CategoryProduct.SingleOrDefault(x => x.ID_Category == cat.ID_Category);
                        if (dbCat == null) return new Data.DbResponse<CategoryProduct>(cat, Data.DbState.NOT_FOUND);

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

            return new Data.DbResponse<CategoryProduct>(cat, _status);
        }

        /// <summary>
        /// Supprime une catégorie de produit
        /// </summary>
        /// <param name="cat"></param>
        /// <returns></returns>
        public Data.DbResponse<CategoryProduct> DeleteCat(CategoryProduct cat)
        {
            if (cat == null) return new Data.DbResponse<CategoryProduct>(cat, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    db.CategoryProduct.Remove(cat);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - DeleteCat] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<CategoryProduct>(cat, _status);
        }
        #endregion

        #region Part de produit
        /// <summary>
        /// Retour la liste complète de toutes type de part
        /// </summary>
        /// <returns></returns>
        public DbResponse<IEnumerable<Portion>> GetAllPart()
        {
            IEnumerable<Portion> parts = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    parts = db.Portion.ToList();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetAllPart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<Portion>>(parts, _status);
        }

        /// <summary>
        /// Récupère une type de part par son ID
        /// </summary>
        /// <param name="ID_Produit"></param>
        /// <returns></returns>
        public DbResponse<Portion> GetPart(int ID_Part)
        {
            Portion part = null;
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    part = db.Portion.FirstOrDefault(x => x.ID_Portion == ID_Part);
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetPart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Portion>(part, _status);
        }

        /// <summary>
        /// Sauvegarde une part en base
        /// Il peut d'agir d'une insertion ou d'une édition
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        public DbResponse<Portion> SavePart(Portion part)
        {
            if (part == null) return new Data.DbResponse<Portion>(part, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    if (!part.Exist())
                    {
                        db.Portion.Add(part);
                    }
                    else
                    {
                        var dbPart = db.Portion.SingleOrDefault(x => x.ID_Portion == part.ID_Portion);
                        if (dbPart == null) return new Data.DbResponse<Portion>(part, Data.DbState.NOT_FOUND);

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

            return new Data.DbResponse<Portion>(part, _status);
        }

        /// <summary>
        /// Supprime une part en base
        /// </summary>
        /// <param name="produit"></param>
        public DbResponse<Portion> DeletePart(Portion part)
        {
            if (part == null) return new Data.DbResponse<Portion>(part, Data.DbState.INVALID_INPUT);
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    db.Portion.Remove(part);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - DeletePart] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<Portion>(part, _status);
        }

        /// <summary>
        /// Retourne la liste des produits et leur catégorie sous forme de liste simple pour être transformé en Tree
        /// </summary>
        /// <returns></returns>
        public DbResponse<IEnumerable<INode>> GetFlatCategorieProduit()
        {
            List<INode> nodes = new List<INode>();
            Data.DbState _status = Data.DbState.OK;

            try
            {
                using (var db = new Data.BakeryContext())
                {
                    var produits = db.Products
                                .Include(x => x.Category)
                                .ToList();
                    var catProduits = db.CategoryProduct
                                .Include(x => x.Parent)
                                .ToList();

                    if(produits != null)
                        nodes.AddRange(produits.Cast<INode>());
                    if (catProduits != null)
                        nodes.AddRange(catProduits.Cast<INode>());
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ProduitServices - GetFlatCategorieProduit] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<IEnumerable<INode>>(nodes, _status);
        }
        #endregion

        /// <summary>
        /// Classe de critère de recherche des produits
        /// </summary>
        public class SearchArgument
        {
            public string name { get; set; }
        }
    }
}
