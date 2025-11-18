using ChatInRealTime.Core.Dtos.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Interfaces
{
    public interface IChatService
    {
        public Task SaveMessage(MessageDto message);
        public Task<IEnumerable<MessageDto>> GetMessages(int count);
    }
}
