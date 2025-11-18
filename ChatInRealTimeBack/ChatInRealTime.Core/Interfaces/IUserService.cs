using ChatInRealTime.Core.Dtos.Responses;
using ChatInRealTime.Core.Dtos.Users;
using ChatInRealTime.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto?> GetById(int Id);
        public Task<AppUser?> GetByEmail(string email);
        public Task<ServiceResponse> Create(SignUpUserDto user);
    }
}
