using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoolBookLatest.Models
{
    public class GenresModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(35, MinimumLength = 1, ErrorMessage = "Please write the name between 1 and 35 characters long")]
        public string Name { get; set; }

        [Required]
        [StringLength(300, MinimumLength = 11, ErrorMessage = "Please write the description between 11 and 300 characters long")]

        public string Description { get; set; }
   

        [Required (ErrorMessage ="Please enter a date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Registration Date")]
        public System.DateTime Created { get; set; }
        
           
        public bool IsDeleted { get; set; }

        
        public string GetShortDate()
        {
            return Created.ToShortDateString();
        }

        
    }
}