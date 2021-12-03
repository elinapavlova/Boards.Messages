using System.Collections.Generic;
using Common.Base;

namespace Database.Models
{
    public class ThreadModel : BaseModel
    {
        public ICollection<MessageModel> Messages { get; set; }
    }
}