using System;
using Common.Base;

namespace Database.Models
{
    public class FileModel : BaseModel
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public Guid ThreadId { get; set; }
        public Guid? MessageId { get; set; }

        public ThreadModel Thread { get; set; }
        public MessageModel Message { get; set; }
    }
}