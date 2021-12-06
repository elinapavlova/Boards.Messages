using System.Collections.Generic;
using Boards.Common.Base;

namespace Boards.MessageService.Database.Models
{
    public class ThreadModel : BaseModel
    {
        public ICollection<MessageModel> Messages { get; set; }
    }
}