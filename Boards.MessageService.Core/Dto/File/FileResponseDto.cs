using System;
using Boards.Common.Base;

namespace Boards.MessageService.Core.Dto.File
{
    public class FileResponseDto : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }

        public Guid ThreadId { get; set; }
        public Guid MessageId { get; set; }
    }
}