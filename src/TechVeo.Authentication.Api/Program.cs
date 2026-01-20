using TechVeo.Authentication.Application;
using TechVeo.Authentication.Infra;
using TechVeo.Authentication.Infra.Persistence.Contexts;
using TechVeo.Shared.Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
    builder.Services.AddPresentation(builder.Configuration, new PresentationOptions
    {
        AddSwagger = true,
        AddJwtAuthentication = true,
        SwaggerTitle = "TechVeo Authentication API V1",
        SwaggerDescription = "TechVeo Authentication API V1"
    });

    builder.Services.AddApplication();
    builder.Services.AddInfra();
}

var app = builder.Build();
{
    app.RunMigration<AuthContext>();

    app.UsePathBase("/auth");

    app.UseForwardedHeaders();

    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
        app.UseHttpsRedirection();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger(options =>
        {
            options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
        });
    }

    app.UseSwaggerUI();

    app.UseInfra();

    app.UseRouting();

    app.UseCors();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
