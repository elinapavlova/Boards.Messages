using System;
using Common.Base;

namespace Core.Dto.Message
{
    public class MessageModelResponseDto : BaseModel
    {
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
    }
}