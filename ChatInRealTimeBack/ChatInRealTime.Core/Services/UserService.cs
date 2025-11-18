using AutoMapper;
using ChatInRealTime.Core.Dtos.Responses;
using ChatInRealTime.Core.Dtos.Users;
using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Core.Specififcation;
using Microsoft.AspNetCore.Identity;


namespace ChatInRealTime.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<AppUser> _userRepo;
        private readonly IMapper _mapper;
       
        public UserService(IRepository<AppUser> userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
           
        }

        public async Task<ServiceResponse> Create(SignUpUserDto user)
        {
            var mappedUser = _mapper.Map<AppUser>(user);
            mappedUser.PasswordHash =
              new PasswordHasher<AppUser>().HashPassword(mappedUser, user.Password);
            try
            {
                await _userRepo.Insert(mappedUser);
                await _userRepo.Save();

                return new ServiceResponse
                {
                    Success = true,
                    Message = "User Created"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }

        }

        public async Task<AppUser?> GetByEmail(string email)
        {
            return await _userRepo.GetItemBySpec(new UserSpecification.FindByEmail(email));
        }
        public async Task<UserDto?> GetById(int Id)
        {
            var user = await _userRepo.GetByID(Id);
            if (user != null)
            {
                var mappedUser = _mapper.Map<UserDto>(user);
                return mappedUser;
            }
            return null;
        }
        internal async Task<AppUser?> GetAppUserById(int Id)
        {
            return await _userRepo.GetByID(Id);
        }
    }
}
