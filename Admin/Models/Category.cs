using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Admin.Models
{
    public class Category
    {
            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string ParentCategoryName { get; set; }
            public string CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
             public string LastUpdatedBy { get; set; }
            public Nullable<DateTime> LastUpdatedDate { get; set; }

    }
}