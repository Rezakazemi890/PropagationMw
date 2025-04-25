using Microsoft.AspNetCore.Mvc;
using PropagationMw.Clients;

namespace PropagationMw.Controllers;

[ApiController]
[Route("[controller]")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly ISampleClient _httpClient;

    public SampleController(ILogger<SampleController> logger, ISampleClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    [HttpGet(Name = "Sample")]
    public IActionResult Get()
    {
        return Ok(_httpClient.GetAsync());
    }
}