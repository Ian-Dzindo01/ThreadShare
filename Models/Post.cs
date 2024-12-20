﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ThreadShare.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int ForumId { get; set; }

        [ForeignKey("ForumId")]
        [JsonIgnore]
        public Forum Forum { get; set; }

        // One-to-many relationship with Comment class
        public ICollection<Comment> Comments { get; set; }

        public DateTime DateCreated { get; set; }

        public Post()
        {
            DateCreated = DateTime.UtcNow;
        }

        public DateTime UpdatedAt { get; set; }
    }
}