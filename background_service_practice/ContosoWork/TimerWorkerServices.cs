using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoWork
{
    public class TimerWorkerServices : IHostedService, IDisposable
    {

        private readonly ILogger<TimerWorkerServices> _logger;
        private Timer _timer;
        public TimerWorkerServices(ILogger<TimerWorkerServices> logger) {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(OnTimer, cancellationToken, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            return Task.CompletedTask; // do something
        }
        private void OnTimer(object state) {
            _logger.LogInformation("OnTimer evnet called");
        }
        public Task StopAsync(CancellationToken canllationToke)
        {
            _timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask; // do something

        }

        public void Dispose() {
            _logger.LogInformation("Dispose called");
            _timer?.Dispose();
        }
    }
}