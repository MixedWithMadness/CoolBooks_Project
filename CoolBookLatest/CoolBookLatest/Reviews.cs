//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoolBookLatest
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reviews
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Nullable<byte> Rating { get; set; }
        public System.DateTime Created { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual AspNetUsers AspNetUsers { get; set; }
        public virtual Books Books { get; set; }
    }
}