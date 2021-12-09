using System;
using System.Collections.Generic;
using Boards.MessageService.Core.Dto.File;
using Boards.Common.Base;

namespace Boards.MessageService.Core.Dto.Message
{
    public class MessageModelDto : BaseModel
    {
        public string Text { get; set; }
        public ICollection<FileResultDto> Files { get; set; }
        public ReferenceToMessageDto ReferenceToMessage { get; set; }
        public Guid ThreadId { get; set; }
    }
}