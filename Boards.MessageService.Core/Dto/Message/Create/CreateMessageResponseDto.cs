using System;
using System.Collections.Generic;
using Boards.MessageService.Core.Dto.File;
using Boards.Common.Base;

namespace Boards.MessageService.Core.Dto.Message.Create
{
    public class CreateMessageResponseDto : BaseModel
    {
        public string Text { get; set; }
        public ICollection<FileResponseDto> Files { get; set; }
        public Guid ThreadId { get; set; }
        public Guid? ReferenceToMessage { get; set; }
    }
}