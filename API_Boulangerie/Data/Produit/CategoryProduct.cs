using API_Orders.Services;
using Shared_Orders.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace API_Orders
{
    public class CategoryProduct : INode, Data.ICopy<CategoryProduct>
    {
        private static ServicesFactory _services => ServicesFactory.Instance;

        [Key]
        public int ID_Category { get; set; }
        [Required]
        public string? Name { get; set; }

        public int? ID_ParentCategory { get; set; }
        [ForeignKey("ID_ParentCategory")]
        public CategoryProduct? Parent { get; set; }

        #region Interface tree
        [NotMapped]
        public int InternalID => ID_Category;

        [NotMapped]
        public string ID_Node => $"CategorieProduit_{ID_Category}";
        [NotMapped]
        public bool IsLeaf => false;
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


        public void Copy(CategoryProduct other)
        {
            this.Name = other.Name;
            this.Parent = other.Parent;
            this.ID_ParentCategory = other.Parent?.ID_Category;
        }

        public bool Exist()
        {
            return ID_Category > 0;
        }

        #region DTO
        public static ProductCategoryDTO FromBusinessObject(CategoryProduct cat)
        {
            if (cat == null) return null;
            return new ProductCategoryDTO()
            {
                ID_category = cat.ID_Category,
                Name = cat.Name,
                Cat = FromBusinessObject(cat?.Parent)
            };
        }

        public static CategoryProduct ToBusinessObject(ProductCategoryDTO catDTO)
        {
            return new CategoryProduct()
            {
                ID_Category = catDTO.ID_category,
                Name = catDTO.Name,
                Parent = ToBusinessObject(catDTO.Cat)
            };
        }

        public void Save()
        {
            _services.Produits.SaveCat(this);
        }

        public void Delete()
        {
            _services.Produits.DeleteCat(this);
        }
        #endregion
    }
}
