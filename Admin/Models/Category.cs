using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class Category
    {
            public int CategoryId { get; set; }

            [Required(ErrorMessage = "Category Name is empty!")]
            [DisplayName("Category Name")]
            public string CategoryName { get; set; }
            public string ParentCategoryName { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
             public string LastUpdatedBy { get; set; }
            public Nullable<DateTime> LastUpdatedDate { get; set; }

    }
}