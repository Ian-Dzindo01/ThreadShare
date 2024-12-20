﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ThreadShare.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        //[JsonIgnore]
        public ICollection<Post> Posts { get; set; }

        public DateTime DateCreated { get; set; }

        public Forum()
        {
            DateCreated = DateTime.UtcNow;
        }

        public DateTime UpdatedAt { get; set; }
    }
}
