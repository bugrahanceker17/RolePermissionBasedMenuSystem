using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using RoleBasedMenuSystem.Core.Entities.Concrete;
using RoleBasedMenuSystem.Core.Extensions;
using RoleBasedMenuSystem.Core.Security.Encryption;
using RoleBasedMenuSystem.Core.Security.Hashing;
using RoleBasedMenuSystem.Core.Security.JWT;
using RoleBasedMenuSystem.Core.Utilities.Attribute;
using RoleBasedMenuSystem.DataAccess.Concrete;
using RoleBasedMenuSystem.DataAccess.Seed;
using RoleBasedMenuSystem.Infrastructure.Abstract.Command;
using RoleBasedMenuSystem.Infrastructure.Abstract.Query;
using RoleBasedMenuSystem.Infrastructure.Abstract.Service;
using RoleBasedMenuSystem.Infrastructure.Concrete.Command;
using RoleBasedMenuSystem.Infrastructure.Concrete.Query;
using RoleBasedMenuSystem.Infrastructure.Concrete.Service;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<PermissionCheckFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.SwaggerRegister("RoleBaseMenuSystem", "v1");

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Database:ConnectionString"));
});

TokenOptionsModel? tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptionsModel>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions?.Issuer,
            ValidAudience = tokenOptions?.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,
            NameClaimType = ClaimTypes.Name,
            RoleClaimType = ClaimTypes.Role,
            SaveSigninToken = true
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IDynamicQuery, DynamicQuery>();
builder.Services.AddScoped<IUserQuery, UserQuery>();
builder.Services.AddScoped<ITokenHelper, JwtHelper>();
builder.Services.AddScoped<IDynamicCommand, DynamicCommand>();
builder.Services.AddScoped<IRelationService, RelationService>();
builder.Services.AddScoped<IUserCommand, UserCommand>();

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(opt =>
        opt.WithOrigins("*").AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
);

builder.Services.AddAutoMapper(typeof(Program));

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
   
}

using (IServiceScope scope = app.Services.CreateScope())
{
    ProjectDbContext context = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();
    await context.Database.MigrateAsync();
    context.PermissionSeedOnRun();

    var adminRoleId = Guid.Empty;
    var adminUserId = Guid.Empty;

    if (!context.AppRoles.Any())
    {
        context.AppRoles.Add( new() { Name = "Standard", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true});
        context.SaveChanges();
        var adminRole = context.AppRoles.Add( new() { Name = "Admin", CreatedAt = DateTime.Now, IsDeleted = false, IsStatus = true});
        context.SaveChanges();

        adminRoleId = adminRole.Entity.Id;
    }
                    
    if (!context.AppUsers.Any())
    {
        byte[] passwordHash, passwordSalt;
        HashingHelper.CreatePasswordHash("pAssw0rd1.&", out passwordHash, out passwordSalt);
        
        AppUser entity = new AppUser
        {
            UserName = "admin",
            FirstName = "System",
            LastName = "Admin",
            
            IsDeleted = false,
            IsStatus = true,
            CreatedAt = DateTime.Now,
            EmailConfirmed = false,
            PhoneNumberConfirmed = false,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        EntityEntry<AppUser> adminUser = context.AppUsers.Add(entity);
        context.SaveChanges();

        adminUserId = adminUser.Entity.Id;
    }

    if (!context.AppUserRoles.Any())
    {
        context.AppUserRoles.Add( new()
        {
            RoleId = adminRoleId,
            UserId = adminUserId,
            CreatedAt = DateTime.Now, 
            IsDeleted = false, 
            IsStatus = true
        });
        
        context.SaveChanges();
    }
}

app.UseHsts();

app.SwaggerConfigure("RoleBaseMenuSystem", "v1");

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();