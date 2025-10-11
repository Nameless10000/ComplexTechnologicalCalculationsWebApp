using System.Diagnostics;

namespace Core.Services
{
    public class SimpleLoggerService
    {

        public SimpleLoggerService()
        {
        }

        public async Task LogAsync(string action, string result, string? comment)
        {
            var logMessage = $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Action: {action} | Result: {result}";

            if (!string.IsNullOrEmpty(comment))
            {
                logMessage += $" | Comment: {comment}";
            }
            Debug.WriteLine(logMessage);

            await Task.CompletedTask;
        }
    }
}
