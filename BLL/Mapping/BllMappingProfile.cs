using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Mapping
{
    public class BllMappingProfile : Profile
    {
        public BllMappingProfile()
        {
            CreateMap<File, FileDto>()
                .ForMember(dto => dto.Downloads,
                    ent => ent.MapFrom(e => e.Statistics.Downloads))
                .ForMember(dto => dto.Views,
                    ent => ent.MapFrom(e => e.Statistics.Views))
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Password,
                    ent => ent.Ignore());

            CreateMap<User, PublicUserInfoDto>();

            CreateMap<UserDto, User>()
                .ForMember(ent => ent.Email,
                    dto => dto.MapFrom(d => d.Email))
                .ForMember(ent => ent.UserName,
                    dto => dto.MapFrom(d => d.UserName))
                .ForAllOtherMembers(ent => ent.Ignore());
        }
    }
}
