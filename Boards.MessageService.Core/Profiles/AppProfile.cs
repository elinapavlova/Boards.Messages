using System.Collections.Generic;
using AutoMapper;
using Boards.MessageService.Core.Dto.File;
using Boards.MessageService.Core.Dto.Message;
using Boards.MessageService.Core.Dto.Message.Create;
using Boards.Common.Result;
using Boards.MessageService.Database.Models;

namespace Boards.MessageService.Core.Profiles
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<CreateMessageRequestDto, MessageModel>()
                .ForMember(x => x.Files, opt =>
                    opt.Ignore());

            CreateMap<FileModel, FileResponseDto>();
            
            CreateMap<FileResponseDto, FileModel>();

            CreateMap<MessageModel, ReferenceToMessageDto>();
            CreateMap<MessageModel, MessageModelResponseDto>();
            CreateMap<MessageModel, CreateMessageResponseDto>();
            CreateMap<MessageModel, MessageModelDto>()
                .ForPath(x => x.ReferenceToMessage.Id, opt =>
                    opt.MapFrom(m => m.ReferenceToMessage));
            CreateMap<MessageModel, ResultContainer<MessageModelDto>>()
                .ForMember(x => x.Data, opt =>
                    opt.MapFrom(m => m));
            CreateMap<MessageModel, ResultContainer<CreateMessageResponseDto>>()
                .ForMember(x => x.Data, opt =>
                    opt.MapFrom(m => m));
            CreateMap<MessageModel, ResultContainer<MessageModelResponseDto>>()
                .ForMember(x => x.Data, opt =>
                    opt.MapFrom(m => m));
            CreateMap<ICollection<MessageModel>, ResultContainer<ICollection<MessageModelDto>>>()
                .ForMember(x => x.Data, opt =>
                    opt.MapFrom(m => m));
        }
    }
}