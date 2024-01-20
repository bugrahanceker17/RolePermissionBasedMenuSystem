using Microsoft.AspNetCore.Mvc;
using RoleBasedMenuSystem.Core.Utilities.Response;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Model.DataTransferObjects.MenuRolePermission;
using RoleBasedMenuSystem.Model.DataTransferObjects.RolePermissionRelation;

namespace RoleBasedMenuSystem.Controllers;

public class RelationController : Controller
{
   private readonly IRelationService _relationService;

   public RelationController(IRelationService relationService)
   {
      _relationService = relationService;
   }

   [HttpPost("set-role-permission-to-menu")]
   public async Task<IActionResult> SetRolePermissionToMenuAsync([FromBody] SetRolePermissionToMenuRequest request)
   {
      DataResult dataResult = await _relationService.SetRolePermissionToMenuAsync(request);
      return dataResult.HttpResponse();
   }
   
   [HttpPost("set-permission-to-role")]
   public async Task<IActionResult> SetPermissionToRoleAsync([FromBody] SetPermissionToRoleRequest request)
   {
      DataResult dataResult = await _relationService.SetPermissionToRoleAsync(request);
      return dataResult.HttpResponse();
   }
}