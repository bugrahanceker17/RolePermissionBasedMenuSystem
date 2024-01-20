using Microsoft.AspNetCore.Mvc;
using RoleBasedMenuSystem.Core.Utilities.Attribute;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.Menu;

namespace RoleBasedMenuSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : Controller
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }
    
    [CustomPermissionControl(true)]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        DataResult dataResult = await _menuService.GetAllAsync();
        return dataResult.HttpResponse();
    }
    
    [CustomPermissionControl(true)]
    [HttpGet("menu-by-role")]
    public async Task<IActionResult> GetAllMenuByPermissionAsync()
    {
        DataResult dataResult = await _menuService.GetAllMenuByPermissionAsync();
        return dataResult.HttpResponse();
    }

    [CustomPermissionControl(true, PermissionsCode.ProjeMenusuGoruntuleme)]
    [HttpPost]
    public async Task<IActionResult> CreateMenuAsync([FromBody] CreateMenuRequest request)
    {
        DataResult dataResult = await _menuService.CreateMenuAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateMenuAsync([FromBody] UpdateMenuRequest request)
    {
        DataResult dataResult = await _menuService.UpdateMenuAsync(request);
        return dataResult.HttpResponse();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteMenuAsync([FromQuery] Guid id)
    {
        DataResult dataResult = await _menuService.DeleteMenuAsync(id);
        return dataResult.HttpResponse();
    }
}