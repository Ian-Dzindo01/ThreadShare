﻿namespace ThreadShare.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }

        public User()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}

