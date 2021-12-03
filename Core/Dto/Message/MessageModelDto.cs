using System;
using System.Collections.Generic;
using Common.Base;
using Core.Dto.File;

namespace Core.Dto.Message
{
    public class MessageModelDto : BaseModel
    {
        public string Text { get; set; }
        public ICollection<FileResponseDto> Files { get; set; }
        public ReferenceToMessageDto ReferenceToMessage { get; set; }
        public Guid ThreadId { get; set; }
    }
}