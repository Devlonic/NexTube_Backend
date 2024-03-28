using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using WebShop.Domain.Constants;

namespace NexTube.Infrastructure.Hubs {
    [Authorize(Roles = Roles.User)]
    public class ProgressReportHub : Hub {
        private ILogger<ProgressReportHub> _logger;
        public ProgressReportHub(ILogger<ProgressReportHub> logger) {
            _logger = logger;
        }
    }
}
