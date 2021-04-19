// TimedBackgroundService.cs by Gradzka & Kazimierczak

namespace LeonCam2.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using LeonCam2.Models;
    using LeonCam2.Services.JwtTokens;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    // From: https://docs.microsoft.com/pl-pl/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-3.1&tabs=visual-studio#timed-background-tasks
    public class TimedBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<TimedBackgroundService> logger;
        private readonly IJwtTokenService jwtTokenService;
        private readonly Settings settings;
        private Timer timer;

        public TimedBackgroundService(ILogger<TimedBackgroundService> logger, IOptions<Settings> settings, IJwtTokenService jwtTokenService)
        {
            this.logger = logger;
            this.jwtTokenService = jwtTokenService;
            this.settings = settings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(TimedBackgroundService)} is starting.");

            this.timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(this.settings.BlockedListControlIntervalInHours));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{nameof(TimedBackgroundService)} is stopping.");

            this.timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            this.timer?.Dispose();
        }

        private void DoWork(object state)
        {
            int removed = this.jwtTokenService.RemoveInvalidTokensFromBlockedList();
            this.logger.LogTrace($"Removed {removed} jwtTokens from blockedlist");
        }
    }
}
