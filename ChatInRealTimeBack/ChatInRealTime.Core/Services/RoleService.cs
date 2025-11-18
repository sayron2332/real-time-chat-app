using ChatInRealTime.Core.Dtos.Responses;
using ChatInRealTime.Core.Entites;
using ChatInRealTime.Core.Interfaces;
using ChatInRealTime.Core.Specififcation;
using Microsoft.Extensions.Caching.Memory;


namespace ChatInRealTime.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<AppRole> _roleRepo;
        private readonly IMemoryCache _cache;

        public RoleService(IRepository<AppRole> roleRepo, IMemoryCache cache)
        {
            _cache = cache;
            _roleRepo = roleRepo;
        }
        public async Task<AppRole?> GetById(int Id)
        {
            return await _roleRepo.GetByID(Id);
        }
        public async Task<AppRole?> GetRoleByNameAsync(string roleName)
        {
            if (!_cache.TryGetValue(roleName, out AppRole? role))
            {
                role = await _roleRepo.GetItemBySpec(new RoleSpesification.GetByName(roleName));

                if (role != null)
                {
                    _cache.Set(roleName, role, TimeSpan.FromHours(1));
                }
            }

            return role;
        }
        public async Task<ServiceResponse> GetAll()
        {
            return new ServiceResponse
            {
                Success = true,
                Payload = await _roleRepo.GetAll(),
                Message = "All roles Load"
            };
        }
    }
}

