using System;
using System.Collections.Generic;

namespace Admin.Models
{
    public class CategoryTree
    {
        public string ParentCategoryName { get; set; }
        public List<Category> Categories { get; set; }
        
    }
}