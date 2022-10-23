using Microsoft.Extensions.Logging;

namespace API.Controllers;

public class ControllerBase
{
    protected readonly ILogger _logger;

    public ControllerBase(ILogger logger)
    {
        _logger = logger;
    }
}
