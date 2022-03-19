using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Orders.DTO
{
    public class ProductCategoryDTO
    {
        public int ID_category { get; set; }
        public string? Name { get; set; }
        public ProductCategoryDTO? Cat { get; set; }
    }
}
