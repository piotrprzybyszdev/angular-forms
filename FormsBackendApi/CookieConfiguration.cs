using Microsoft.AspNetCore.Authentication.Cookies;

namespace FormsBackendBusiness;

public static class CookieConfiguration
{
    public static void Configure(IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = true;
                options.Cookie.HttpOnly = true;
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToAccessDenied = context =>
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnRedirectToLogin = context =>
                    {
                        context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                };
            });
    }
}
