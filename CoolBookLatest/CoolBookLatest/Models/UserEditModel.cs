using CoolBookLatest.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoolBookLatest.Models
{
    public class UserEditModel
    {
      
        public string UserId { get; set; }


        [Required]
        [StringLength(45, MinimumLength = 2)]
        [RegularExpression("([a-z,A-ZöåäÖÅÄ]*)", ErrorMessage ="Only Swedish and English alphabets are accepted")]
        public string FirstName { get; set; }


        [Required]
        [StringLength(45, MinimumLength = 2)]
        [RegularExpression("([a-z,A-ZöåäÖÅÄ]*)", ErrorMessage = "Only Swedish and English alphabets are accepted")]
        public string LastName { get; set; }


        [DisplayName("Gender")]
        public Gender SelectedGender { get; set; }


        [DisplayName("BirthDate")]
        [Required]
        [DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]


        public Nullable<System.DateTime> Birthdate { get; set; }

        public byte[] Picture { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Phone must be a natural number")]

        public string Phone { get; set; }


       
        public string Address { get; set; }

        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "ZipCode must be a valid number")]

        public string ZipCode { get; set; }


        [StringLength(45, MinimumLength = 2)]
        [RegularExpression("([a-z,A-ZöåäÖÅÄ]*)", ErrorMessage = "Only Swedish and English alphabets are accepted")]
        public string City { get; set; }

       
        public Countries Country { get; set; }
    
        public string Info { get; set; }
    }
}