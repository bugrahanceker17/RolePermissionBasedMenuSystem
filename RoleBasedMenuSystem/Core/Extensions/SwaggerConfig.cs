using Microsoft.OpenApi.Models;

namespace RoleBasedMenuSystem.Core.Extensions;

public static class SwaggerConfig
{
    public static void SwaggerConfigure(this IApplicationBuilder app, string assembly, string version)
      {
          app.UseSwagger(options =>
              {
                  options.PreSerializeFilters.Add((swagger, httpReq) =>
                  {
                      if (httpReq.Headers.ContainsKey("X-Forwarded-Host"))
                      {
                          string basePath = $"{assembly}";
                          string serverUrl = $"https://{httpReq.Headers["X-Forwarded-Host"]}/{basePath}";
                          swagger.Servers = new List<OpenApiServer> { new() { Url = serverUrl } };
                      }
                  });
              }
          ).UseSwaggerUI(options =>
              {
                  options.RoutePrefix = "swagger";
                  options.SwaggerEndpoint("v1/swagger.json", $"{assembly} {version}");
              });
      }
  
      public static void SwaggerRegister(this IServiceCollection services, string assembly, string version)
      {
          services.AddSwaggerGen(c =>
          {
              c.SwaggerDoc(version, new OpenApiInfo { Title = assembly, Version = version });
              
              OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
              {
                  Name = "JWT Authentication",
                  Description = "Enter JWT Bearer token **_only_**",
                  In = ParameterLocation.Header,
                  Type = SecuritySchemeType.Http,
                  Scheme = "bearer",
                  BearerFormat = "JWT",
                  Reference = new OpenApiReference
                  {
                      Id = "Bearer",
                      Type = ReferenceType.SecurityScheme
                  }
              };
              c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
              c.AddSecurityRequirement(new OpenApiSecurityRequirement
              {
                  { securityScheme, Array.Empty<string>() }
              });
          });
      }
}