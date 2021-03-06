﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class Product
    {

        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is empty!")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [Required]
        [DisplayName("Brand")]
        public int BrandId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public Nullable<DateTime> LastUpdatedDate { get; set; }
    }
}