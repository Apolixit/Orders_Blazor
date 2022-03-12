using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Services
{
    public interface IProduitServices
    {
        Data.DbResponse<IEnumerable<Produit>> GetAll();
        Data.DbResponse<Produit> Get(int ID_Produit);
        Data.DbResponse<Produit> Save(Produit produit);
        Data.DbResponse<Produit> Delete(Produit produit);

        Data.DbResponse<IEnumerable<Part>> GetAllPart();
        Data.DbResponse<Part> GetPart(int ID_Part);
        Data.DbResponse<Part> SavePart(Part part);
        Data.DbResponse<Part> DeletePart(Part part);

        Data.DbResponse<CategorieProduit> GetCat(int ID_Cat);
        Data.DbResponse<CategorieProduit> SaveCat(CategorieProduit cat);
        Data.DbResponse<CategorieProduit> DeleteCat(CategorieProduit cat);

        Data.DbResponse<IEnumerable<IProduitNode>> GetFlatCategorieProduit();
    }
}
