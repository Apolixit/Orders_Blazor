using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Orders.DTO
{
    /// <summary>
    /// Modèle utilisé par l'api ProduitsController
    /// </summary>
    public class ProductDTO
    {
        public int id_product { get; set; }
        public string? name { get; set; }
        public PortionDTO? portion { get; set; }
        public ProductCategoryDTO? category { get; set; }
    }
}
