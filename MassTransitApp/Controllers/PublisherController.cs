using MassTransit;
using MassTransitApp3.Sagas.States;
using Microsoft.AspNetCore.Mvc;
using ObjectClass;

[ApiController]
[Route("[controller]")]
public class PublisherController : Controller
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<PublisherController> _logger;

    public PublisherController(IPublishEndpoint publishEndpoint, ILogger<PublisherController> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    [HttpPost("publish-objects")]
    public async Task<IActionResult> PublishObjects([FromBody] Objects objects)
    {
        try
        {
            if (objects == null || string.IsNullOrWhiteSpace(objects.ObjectName) || string.IsNullOrWhiteSpace(objects.ObjectDescription))
            {
                _logger.LogError("Object is empty");
                return BadRequest("Object is empty");
            }
            else
            {
                await _publishEndpoint.Publish(objects);
                _logger.LogInformation($"Published object: {objects.ObjectName} {objects.ObjectDescription} successfully!");
                return Ok($"Published object: {objects.ObjectName} {objects.ObjectDescription} successfully!");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("publish-objects-to-saga")]
    public async Task<IActionResult> PublishObjectsToSaga([FromBody] Objects objects)
    {
        try
        {
            if (objects == null || string.IsNullOrWhiteSpace(objects.ObjectName) || string.IsNullOrWhiteSpace(objects.ObjectDescription))
            {
                _logger.LogError("Object is empty");
                return BadRequest("Object is empty");
            }
            else
            {
                var makeObject = new ObjectMade
                {
                    CorrelationId = Guid.NewGuid(),
                    ObjectName = objects.ObjectName,
                    ObjectDescription = objects.ObjectDescription
                };
                await _publishEndpoint.Publish(makeObject);
                _logger.LogInformation($"Published object to saga: {objects.ObjectName} {objects.ObjectDescription} successfully!");
                return Ok($"Published object to saga: {objects.ObjectName} {objects.ObjectDescription} successfully!");
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
