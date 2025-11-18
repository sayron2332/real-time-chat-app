using ChatInRealTime.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Entites
{
    public class AppUser : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public AppRole AppRole { get; set; } = null!;
        public int AppRoleId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
       
    }
}
