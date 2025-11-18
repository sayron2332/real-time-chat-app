using ChatInRealTime.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Entites
{
    public class AppRole : IEntity
    { 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<AppUser> AppUsers { get; set; } = null!;
    }
}
