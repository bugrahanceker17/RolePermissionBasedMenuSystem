using Microsoft.AspNetCore.Mvc;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.Permission;

namespace RoleBasedMenuSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PermissionController : Controller
{
    private readonly IPermissionService _permissionService;

    public PermissionController(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        DataResult dataResult = await _permissionService.GetAllAsync();
        return dataResult.HttpResponse();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePermissionAsync([FromBody] CreatePermissionRequest request)
    {
        DataResult dataResult = await _permissionService.CreatePermissionAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdatePermissionAsync([FromBody] UpdatePermissionRequest request)
    {
        DataResult dataResult = await _permissionService.UpdatePermissionAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeletePermissionAsync([FromQuery] Guid id)
    {
        DataResult dataResult = await _permissionService.DeletePermissionAsync(id);
        return dataResult.HttpResponse();
    }
}