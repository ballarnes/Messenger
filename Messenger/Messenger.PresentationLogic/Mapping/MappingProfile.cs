using AutoMapper;
using Messenger.App.Models;
using Messenger.DataAccess.Models.Dtos;
using Messenger.DataAccess.Models.Entities;

namespace Messenger.PresentationLogic.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<DataAccess.Models.Entities.Message, MessageDto>();
            CreateMap<MessageDto, DataAccess.Models.Entities.Message>();
        }
    }
}