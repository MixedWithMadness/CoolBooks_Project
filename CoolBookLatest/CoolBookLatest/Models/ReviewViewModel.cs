﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CoolBookLatest.Models
{
    public class ReviewViewModel
    {
        
        public int Id { get; set; }
        
        public int BookId { get; set; }
        
        public string UserId { get; set; }
        [Required(ErrorMessage = "Please enter a Title")]
        [MinLength(4)]
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter a summary")]
        public string Text { get; set; }
        [Required]
        [Range(1,10,ErrorMessage ="Plese enter 1-10")]
        public Nullable<byte> Rating { get; set; }
        
        public System.DateTime Created { get; set; }
        public bool IsDeleted { get; set; }

        public static implicit operator ReviewViewModel(Reviews reviews)
        {
            return new ReviewViewModel()
            {
                Id = reviews.Id,
                BookId = reviews.BookId,
                UserId = reviews.UserId,
                Title = reviews.Title,
                Text = reviews.Text,
                Rating = reviews.Rating,
                Created = reviews.Created,
                IsDeleted = reviews.IsDeleted
            };
        }
        public Reviews VMToModel(Reviews review)
        {

            review.Id = this.Id;
            review.BookId = this.BookId;
            review.UserId = this.UserId;
            review.Title = this.Title;
            review.Text = this.Text;
            review.Rating = this.Rating;
            review.Created = this.Created;
            review.IsDeleted = this.IsDeleted;

            return review;
        }
    }
}