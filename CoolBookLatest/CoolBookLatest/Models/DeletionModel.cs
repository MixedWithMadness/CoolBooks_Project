using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoolBookLatest.Models
{
	public class DeletionModel
	{
        public int Id { get; set; }
        public List<Authors> authorsList { get; set; }
        public List<Genres> genresList { get; set; }
        public List<Users> usersList { get; set; }
        public List<Books> BooksList { get; set; }
        public List<Reviews> ReviewsList { get; set; }
    }
}