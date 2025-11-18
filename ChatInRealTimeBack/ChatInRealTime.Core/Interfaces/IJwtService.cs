using ChatInRealTime.Core.Dtos.ServerResponses;
using ChatInRealTime.Core.Dtos.Token;
using ChatInRealTime.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Interfaces
{
    public interface IJwtService
    {
        public Task<TokensDto> CreateTokenResponse(AppUser? user);
        public Task<AuthResponse> RefreshTokensAsync(string token);
    }
}
