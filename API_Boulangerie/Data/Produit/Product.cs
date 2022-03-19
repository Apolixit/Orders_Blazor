using API_Orders.Services;
using API_Orders.Utils.Tree;
using Shared_Orders.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Orders
{
    public class Product : Data.ICopy<Product>, INode
    {
        private static ServicesFactory _services => ServicesFactory.Instance;

        [Key]
        public int ID_Product { get; set; }
        [Required]
        public string? Name { get; set; }
        public int? ID_Portion { get; set; }
        [ForeignKey("ID_Portion")]
        public Portion? PortionProduct { get; set; }
        public string? PathImage { get; set; }
        public int? ID_Category { get; set; }
        [ForeignKey("ID_Category")]
        public CategoryProduct? Category { get; set; }

        #region Interface tree
        [NotMapped]
        public int InternalID => ID_Product;
        [NotMapped]
        public string ID_Node => $"Product_{ID_Product}";
        [NotMapped]
        public bool IsLeaf => true;

        [NotMapped]
        public string Parent_ID
        {
            get
            {
                if (Category == null) return null;
                else return Category.ID_Node;
            }
        }
        #endregion

        public void Copy(Product other)
        {
            this.Name = other.Name;
            this.PathImage = other.PathImage;
        }

        public bool Exist()
        {
            return this.ID_Product > 0;
        }

        #region Database mapping

        public static ProductDTO FromBusinessObject(Product produit)
        {
            if (produit == null) return null;

            return new ProductDTO()
            {
                id_product = produit.ID_Product,
                name = produit.Name,
                category = FromBusinessObject(produit).category,
                portion = FromBusinessObject(produit).portion
            };
        }

        public static Product ToBusinessObject(ProductDTO produitsDTO)
        {
            return new Product()
            {
                ID_Product = produitsDTO.id_product,
                Name = produitsDTO.name,
                //Category = ToBusinessObject(produitsDTO).Category,
                //PortionProduct = ToBusinessObject(produitsDTO).PortionProduct
            };
        }
        #endregion

        public static IEnumerable<ProductDTO> GetAll()
        {
            return _services.Produits.GetAll().data.Select(x => FromBusinessObject(x));
        }

        public static ProductDTO Get(int id)
        {
            return FromBusinessObject(_services.Produits.Get(id).data);
        }

        public static IEnumerable<TreeProduit> AsTree()
        {
            var lProduits = _services.Produits.GetFlatCategorieProduit().data;
            return TreeProduit.Build(TreeItem<INode>.GetTree(lProduits, null));
        }

    }
}
