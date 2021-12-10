using System;
using System.Collections.Generic;
using Boards.Auth.Common.Base;

namespace Boards.MessageService.Core.Dto.Message.Create
{
    public class CreateMessageResponseDto : BaseModel
    {
        public string Text { get; set; }
        public ICollection<Uri> Files { get; set; }
        public Guid ThreadId { get; set; }
        public Guid? ReferenceToMessage { get; set; }
    }
}