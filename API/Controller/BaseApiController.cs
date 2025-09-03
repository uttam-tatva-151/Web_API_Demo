using Microsoft.AspNetCore.Mvc;
using Web_API.Attributes;

namespace Web_API.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ValidateModel]
public abstract class BaseApiController : ControllerBase
{
}
