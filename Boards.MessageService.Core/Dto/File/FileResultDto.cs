using System;

namespace Boards.MessageService.Core.Dto.File
{
    public class FileResultDto
    {
        public byte[] Bytes { get; set; }
        public string ContentType { get; set; }
        public Uri Url { get; set; }
    }
}