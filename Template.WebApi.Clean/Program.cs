using Asp.Versioning;
using Scalar.AspNetCore;
using Template.WebApi.Clean.Infrastructure.Extensions;
using Template.WebApi.Clean.Installers.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddOpenApi();
builder.Services.InstallServicesInAssembly(builder.Configuration);

var app = builder.Build();
app.InstallApplicationInAssembly(builder.Configuration);
app.UseErrorHandlingMiddleware();
app.UseHttpsRedirection();

app.MapControllers();

app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Template.WebApi.Clean");
});

app.Run();
