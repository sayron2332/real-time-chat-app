using AutoMapper;
using ChatInRealTime.Core.Dtos.Message;
using ChatInRealTime.Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Mapper
{
    public class AutoMapperMessageProfile : Profile
    {
        public AutoMapperMessageProfile()
        {
            CreateMap<AppMessage, MessageDto>().ReverseMap();
        }
    }
}
