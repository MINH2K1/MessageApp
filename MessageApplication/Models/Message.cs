﻿namespace MessageApplication.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string ?Content { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}