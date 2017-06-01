using CoolBookLatest;
using CoolBookLatest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CoolBookLatest.Models
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [DisplayName("Author")]
        public int AuthorId { get; set; }
        [DisplayName("Genre")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "A Title is required")]
        [StringLength(255, MinimumLength =3, ErrorMessage ="Please write The titel under between 3 and 255 characters long")]
        public string Title { get; set; }

        [StringLength(255, MinimumLength = 3, ErrorMessage = "Please write The alt.titel under between 3 and 255 characters long")]
        [DisplayName("Alt. Titel")]
        public string AlternativeTitle { get; set; }
        [Range(1,99,ErrorMessage ="Please enter a number between 1 and 99")]
        public Nullable<short> Part { get; set; }


        [Required(ErrorMessage = "A short Description is required")]
        [StringLength (300, MinimumLength =5, ErrorMessage =" Please write between 5 and 300 characters.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }



        [StringLength(25, MinimumLength = 9, ErrorMessage ="Write ISBN between 9 and 25 digits long")]
        [RegularExpression("([0-9,-_]*)", ErrorMessage = "Only digits are dashes are allowed")]
        [Required(ErrorMessage = "An ISBN is required")]
        [Index(IsUnique = true)]
        public string ISBN { get; set; }
        [DisplayName("Publised Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PublishDate { get; set; }

        [DisplayName("Image Path")]
        public string ImagePath { get; set; }

        public virtual Authors Authors { get; set; }
        public virtual Genres Genres { get; set; }

        public virtual ICollection<Reviews> Reviews { get; set; }

        public static implicit operator BookViewModel(Books books)
        {
            return new BookViewModel()
            {
                Id = books.Id,
                AuthorId = books.AuthorId,
                GenreId = books.GenreId,
                Title = books.Title,
                AlternativeTitle = books.AlternativeTitle,
                Part = books.Part,
                Description = books.Description,
                ISBN = books.ISBN,
                PublishDate = books.PublishDate,
                ImagePath = books.ImagePath,
                Authors = books.Authors,
                Genres = books.Genres,
                Reviews = books.Reviews
                
            };
        }

        public void UPloadBookPicture(HttpPostedFileBase fileUpload)
        {
           // fileUpload.FileName;
        }

        public Books VMToBooks(Books books)
        {
            
            books.Id = this.Id;
            books.AuthorId = this.AuthorId;
            books.GenreId = this.GenreId;
            books.Title = this.Title;
            books.AlternativeTitle = this.AlternativeTitle;
            books.Part = this.Part;
            books.Description = this.Description;
            books.ISBN = this.ISBN;
            books.PublishDate = this.PublishDate;
            books.ImagePath = this.ImagePath;

            if (Authors != null)
            {
                books.Authors = this.Authors;
            }
            if (Genres != null)
            {
                books.Genres = this.Genres;
            }
            if(Reviews != null)
            {
                books.Reviews = this.Reviews;
            }

            return books;
        }

    }
}
