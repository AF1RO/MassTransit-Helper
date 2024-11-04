using MassTransit;
using ObjectClass;

namespace MassTransitApp2.Controllers
{
    public class ConsumerController : IConsumer<Objects>
    {
        private readonly ILogger<ConsumerController> _logger;

        public ConsumerController(ILogger<ConsumerController> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<Objects> context)
        {
            try
            {
                _logger.LogInformation($"Consumed Object:" +
                $"\nObject Name: {context.Message.ObjectName}" +
                $"\nObject Description: {context.Message.ObjectDescription}");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Task.FromException(ex);
            }
        }
    }
}