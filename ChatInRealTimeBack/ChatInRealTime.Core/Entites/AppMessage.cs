using ChatInRealTime.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Entites
{
    public class AppMessage : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string UserName { get; set; } = string.Empty;
        public Sentiment? Sentiment { get; set; }
    }
    public enum Sentiment
    {
        Positive = 1,
        Neutral = 0,
        Negative = -1
    }
}
