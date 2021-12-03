using System;
using Microsoft.AspNetCore.Http;

namespace Boards.MessageService.Core.Dto.Message.Create
{
    public class CreateMessageRequestDto
    {
        public string Text { get; set; }
        public IFormFileCollection Files { get; set; }
        public Guid ThreadId { get; set; }
        public Guid? ReferenceToMessage { get; set; }
    }
}