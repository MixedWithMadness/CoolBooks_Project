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
        public string Name { get; set; }
        public string Description { get; set; }

        public string ShortDate { get; set; }
           

        [Required]
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