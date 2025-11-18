using AutoMapper;
using ChatInRealTime.Core.Dtos.Responses;
using ChatInRealTime.Core.Dtos.ServerResponses;
using ChatInRealTime.Core.Dtos.Token;
using ChatInRealTime.Core.Dtos.Users;
using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRoleService _roleService;
        private readonly IJwtService _jwtService;
        private readonly IUserService _userService;
        public AuthService(IJwtService jwtService, IRoleService roleService,
             IUserService userService)
        {

            _jwtService = jwtService;
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<AuthResponse> LoginUser(SignInUserDto model)
        {
            var user = await _userService.GetByEmail(model.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "User not found or email or password incorrect",
                };
            }

            var result = new PasswordHasher<AppUser>()
                .VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return new AuthResponse
                {
                    Message = "Verefication failed pleas check your password",
                    Success = false,
                };

            }


            TokensDto tokens = await _jwtService.CreateTokenResponse(user);
            return new AuthResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                Message = "Logged in successfully",
                Success = true,
            };
        }

        public async Task<ServiceResponse> RegisterUser(SignUpUserDto model)
        {
            var user = await _userService.GetByEmail(model.Email);
            if (user != null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User Already exist"
                };
            }
          
            var role = await _roleService.GetRoleByNameAsync("user");
            if (role != null)
            {
                model.AppRoleId = role.Id;
            }
            ServiceResponse result = await _userService.Create(model);
            return result;
        }
    }
}

