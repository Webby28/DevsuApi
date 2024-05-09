using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("v{version:apiVersion}/api/[controller]")]
public abstract class BaseApiController : Controller
{
    private readonly IMapper _mapper = default;
    protected IMapper Mapper => _mapper ?? HttpContext.RequestServices.GetService<IMapper>();
}