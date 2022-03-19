using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public interface IProductServices
    {
        Data.DbResponse<IEnumerable<Product>> GetAll();
        Data.DbResponse<Product> Get(int ID_Produit);
        Data.DbResponse<Product> Save(Product produit);
        Data.DbResponse<Product> Delete(Product produit);

        Data.DbResponse<IEnumerable<Portion>> GetAllPart();
        Data.DbResponse<Portion> GetPart(int ID_Part);
        Data.DbResponse<Portion> SavePart(Portion part);
        Data.DbResponse<Portion> DeletePart(Portion part);

        Data.DbResponse<CategoryProduct> GetCat(int ID_Cat);
        Data.DbResponse<CategoryProduct> SaveCat(CategoryProduct cat);
        Data.DbResponse<CategoryProduct> DeleteCat(CategoryProduct cat);

        Data.DbResponse<IEnumerable<INode>> GetFlatCategorieProduit();
    }
}
