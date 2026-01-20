using Microsoft.AspNetCore.Builder;

namespace TechVeo.Authentication.Infra
{
    public static class RequestPipeline
    {
        public static IApplicationBuilder UseInfra(this IApplicationBuilder app)
        {
            app.UseSharedInfra();

            return app;
        }
    }
}
