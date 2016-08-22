using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class CategoryParameter
    {
        public int ParameterId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Parameter Name is required!")]
        [DisplayName("Parameter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Parameter Values are required!")]
        [DisplayName("Parameter Values")]

        public string Values { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string LastUpdatedBy { get; set; }

        public Nullable<DateTime> LastUpdatedDate { get; set; }

    }
}