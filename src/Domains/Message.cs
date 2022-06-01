using System;

namespace RabbitMQ.Domains
{
    public class Message
    {
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}