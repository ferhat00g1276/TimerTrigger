using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerTrigger.Services
{
    public interface IQueueService
    {
        Task SendMessageAsync(string message);
        Task SendMessageWithCountAsync(string message, int count);
        Task<string> RecieveMessageAsync();
        Task<List<string>> RecieveMessagesAsync();
        Task<bool> Exists(string message);
    }
}
