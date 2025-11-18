using ChatInRealTime.Core.Dtos.ServerResponses;
using ChatInRealTime.Core.Dtos.Token;
using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Core.Specififcation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<RefreshToken> _tokenRepo;
        private readonly ILogger<JwtService> _logger;
        private readonly UserService _userService;
        private readonly IRoleService _roleService;

        public JwtService(IConfiguration configuration, ILogger<JwtService> logger,
            IRepository<RefreshToken> repository, UserService userService, IRoleService roleService)
        {

            _configuration = configuration;
            _tokenRepo = repository;
            _logger = logger;
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<TokensDto> CreateTokenResponse(AppUser? user)
        {

            if (user != null)
            {
                return new TokensDto
                {
                    AccessToken = await CreateToken(user),
                    RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
                };
            }
            return new TokensDto();
        }
        public async Task<AuthResponse> RefreshTokensAsync(string token)
        {
            var refreshToken = await _tokenRepo
                .GetItemBySpec(new TokenSpecification.GetByToken(token));

            if (refreshToken != null && refreshToken.ExpireTime >= DateTime.UtcNow)
            {
                var user = await _userService.GetAppUserById(refreshToken.UserId);
                if (user != null)
                {
                    return new AuthResponse
                    {
                        Success = true,
                        AccessToken = await CreateToken(user),
                        RefreshToken = refreshToken.Token,
                        Message = "Acces Token Generated"
                    };
                }
                return new AuthResponse
                {
                    Success = false,
                    Message = "user not found"
                };
            }
            return new AuthResponse
            {
                Success = false,
                Message = "Refresh Token not valid"
            };
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(AppUser user)
        {
            RefreshToken refreshToken = new();
            refreshToken.Token = GenerateRefreshToken();
            refreshToken.ExpireTime = DateTime.UtcNow.AddDays(7);
            refreshToken.UserId = user.Id;
            try
            {
                await _tokenRepo.Insert(refreshToken);
                await _tokenRepo.Save();
                return refreshToken.Token;
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Failed to generate and save refresh token for user {UserId}", user.Id);
                return "Some problem with Token";
            }

        }
        private async Task<string> CreateToken(AppUser user)
        {
            var role = await _roleService.GetById(user.AppRoleId);
            user.AppRole = role!;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role!.Name)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtConfig:Secret")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JwtConfig:Issuer"),
                audience: _configuration.GetValue<string>("JwtConfig:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
