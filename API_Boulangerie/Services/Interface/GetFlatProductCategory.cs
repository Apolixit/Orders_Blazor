using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public interface GetFlatProductCategory
    {
        /// <summary>
        /// Get all the products
        /// </summary>
        /// <returns></returns>
        Task<Data.DbResponse<IEnumerable<Product>>> GetAll();
        /// <summary>
        /// Get a product by his ID
        /// </summary>
        /// <param name="ID_Produit"></param>
        /// <returns></returns>
        Task<Data.DbResponse<Product>> Get(int ID_Produit);
        /// <summary>
        /// Search a product
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        Task<Data.DbResponse<Product>> Save(Product produit);
        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="produit"></param>
        /// <returns></returns>
        Task<Data.DbResponse<Product>> Delete(Product produit);
        
        Task<Data.DbResponse<IEnumerable<Portion>>> GetAllPart();
        Task<Data.DbResponse<Portion>> GetPart(int ID_Part);
        Task<Data.DbResponse<Portion>> SavePart(Portion part);
        Task<Data.DbResponse<Portion>> DeletePart(Portion part);
        
        Task<Data.DbResponse<CategoryProduct>> GetCat(int ID_Cat);
        Task<Data.DbResponse<CategoryProduct>> SaveCat(CategoryProduct cat);
        Task<Data.DbResponse<CategoryProduct>> DeleteCat(CategoryProduct cat);

        Task<Data.DbResponse<IEnumerable<INode>>> GetFlatProductCategory();
    }
}
