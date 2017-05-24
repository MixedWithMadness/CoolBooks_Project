using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoolBookLatest.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Nullable<byte> Rating { get; set; }
        public System.DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    }
}