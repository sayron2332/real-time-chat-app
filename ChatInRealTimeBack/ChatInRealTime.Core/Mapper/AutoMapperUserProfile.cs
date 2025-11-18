using AutoMapper;
using ChatInRealTime.Core.Dtos.Users;
using ChatInRealTime.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Mapper
{
    public class AutoMapperUserProfile : Profile
    {
        public AutoMapperUserProfile()
        {
            CreateMap<SignUpUserDto, AppUser>();
            CreateMap<AppUser, UserDto>().ReverseMap();
        }
    }
}
