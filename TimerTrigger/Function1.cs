using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimerTrigger.Services;

namespace TimerTrigger
{
    public class Function1
    {
        private readonly ILogger _logger;
        private readonly IQueueService _queueService;
        public Function1(ILoggerFactory loggerFactory, IQueueService queueService)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
            _queueService = queueService;
        }
        
        [Function("Function1")]
        public void Run([TimerTrigger("*/2 * * * * *")] TimerInfo myTimer)
        {
           
            _queueService.SendMessageAsync($"C# Timer trigger function executed at: {DateTime.Now}");
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
        
        [Function("Function2")]
        public async Task Run2([TimerTrigger("*/6 * * * * *")] TimerInfo myTimer)
        {
       
            var messages = await _queueService.RecieveMessagesAsync();

            string messageString = "";

            foreach (var message in messages)
            {
                messageString += message + "\n";
            }
            _logger.LogInformation(messageString);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
