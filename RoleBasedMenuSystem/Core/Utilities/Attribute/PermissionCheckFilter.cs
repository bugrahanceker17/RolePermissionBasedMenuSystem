using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using RoleBasedMenuSystem.Core.Extensions;
using RoleBasedMenuSystem.DataAccess.Concrete;
using RoleBasedMenuSystem.Model.Entities;

namespace RoleBasedMenuSystem.Core.Utilities.Attribute;

public class PermissionCheckFilter : IAuthorizationFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public PermissionCheckFilter(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.Any(em => em is CustomPermissionControlAttribute))
        {
            CustomPermissionControlAttribute? customAttribute = context.ActionDescriptor.EndpointMetadata.OfType<CustomPermissionControlAttribute>().FirstOrDefault();

            List<string> permissionParams = customAttribute?.Permissions?.ToList();
            bool? loginCheck = customAttribute?.MustLogin;

            if (loginCheck.HasValue && loginCheck.Value)
            {
                if (string.IsNullOrEmpty(_httpContextAccessor.AccessToken().accessToken))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }

            if (permissionParams != null && permissionParams.Any())
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                HttpRequest request = httpContext?.Request;

                string accessToken = request?.Headers["Authorization"].FirstOrDefault();

                if (string.IsNullOrEmpty(accessToken))
                    if (request.Query.TryGetValue("access_token", out StringValues token))
                        accessToken = $"Bearer {token}";

                List<string> roles = new();
                List<Claim> claims = null;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    if (accessToken.StartsWith("Bearer")) accessToken = accessToken.Replace("Bearer ", "").Replace("\"", "").Trim();

                    JwtSecurityToken securityToken = new JwtSecurityToken(accessToken);

                    if (securityToken.Claims.Any())
                    {
                        roles = securityToken.Claims.Where(c => c.Type == "sub").ToList().Select(c => c.Value).ToList();
                    }
                }

                if (roles.Any())
                {
                    List<Guid> guidRoles = new();
                    roles.ForEach(item => { guidRoles.Add(new Guid(item)); });

                    using (var dbContext = new ProjectDbContext(_configuration))
                    {
                        List<RolePermissionRelationEntity> rolePermissionRelation = dbContext.RolePermissionRelations.ToList()
                            .Where(c => guidRoles.Contains(c.RoleId) && c is { IsStatus: true, IsDeleted: false }).ToList();

                        List<string> allPermissionInRoles = rolePermissionRelation.Select(c => c.PermissionId.ToString()).ToList();

                        IEnumerable<string> permissions = dbContext.Permissions.ToList()
                            .Where(c => permissionParams.Contains(c.Code.ToString()))
                            .ToList().Select(c => c.Id.ToString());

                        if (permissions.Any(c => allPermissionInRoles.Contains(c)))
                            Console.WriteLine("Yetki kontrolü başarılı !");
                        
                        else
                            context.Result = new ObjectResult("Yetkilendirme başarısız") { StatusCode = 403 };
                    }
                }
            }
        }
    }
}