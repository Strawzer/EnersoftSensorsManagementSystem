using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnersoftSensorsManagementSystem.API.Controllers;

[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Route("api/[controller]")]
[Authorize(Roles = "User")]
public class SensorsV2Controller : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok("This is version 2.0 of the GetAllSensors endpoint.");
    }
}
