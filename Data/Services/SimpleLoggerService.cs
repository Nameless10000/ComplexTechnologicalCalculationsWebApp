using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Services
{
    public class SimpleLoggerService : ISimpleLoggerService
    {
        private readonly ILogger<SimpleLoggerService> _logger;

        public SimpleLoggerService(ILogger<SimpleLoggerService> logger)
        {
            _logger = logger;
        }

        public async Task LogAsync(string action, string result, string comment = null)
        {
            try
            {
                var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Action: {action} | Result: {result}";

                if (!string.IsNullOrEmpty(comment))
                {
                    logMessage += $" | Comment: {comment}";
                }

                Debug.WriteLine(logMessage);

                _logger.LogInformation(logMessage);


                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging");
            }
        }
    }
}
