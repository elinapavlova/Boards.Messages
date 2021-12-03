﻿using System;
using System.Collections.Generic;
using Common.Base;

namespace Database.Models
{
    public class MessageModel : BaseModel
    {
        public string Text { get; set; }
        public Guid ThreadId { get; set; }
        public Guid? ReferenceToMessage { get; set; }
        
        public ThreadModel Thread { get; set; }
        public ICollection<FileModel> Files { get; set; } 
    }
}