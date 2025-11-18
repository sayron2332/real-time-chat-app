using ChatInRealTime.Core.Dtos.Responses;
using ChatInRealTime.Core.Dtos.ServerResponses;
using ChatInRealTime.Core.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponse> LoginUser(SignInUserDto model);
        public Task<ServiceResponse> RegisterUser(SignUpUserDto model);
    }
}
