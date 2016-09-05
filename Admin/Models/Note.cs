using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class Note
    {
        public int NoteId { get; set; }

        [Required(ErrorMessage = "Description is empty!")]
        [DisplayName("Description")]
        public string NoteText { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}