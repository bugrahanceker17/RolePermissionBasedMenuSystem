using Microsoft.AspNetCore.Mvc;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.Auth;

namespace RoleBasedMenuSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        DataResult dataResult = await _authService.RegisterAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        DataResult dataResult = await _authService.LoginAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpGet]
    [Route("role-list")]
    public async Task<IActionResult> GetAllRoleAsync()
    {
        DataResult dataResult = await _authService.GetAllRoleAsync();
        return dataResult.HttpResponse();
    }
    
    [HttpPost]
    [Route("create-role")]
    public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleRequest request)
    {
        DataResult dataResult = await _authService.AddRoleAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpPost]
    [Route("add-role-to-user")]
    public async Task<IActionResult> AddRoleToUserAsync([FromBody] AddRoleToUserRequest request)
    {
        DataResult dataResult = await _authService.AddRoleToUserAsync(request);
        return dataResult.HttpResponse();
    }
}