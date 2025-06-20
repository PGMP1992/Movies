using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Movies.Business.Repos;
using Movies.Business.Repos.Interfaces;
using Movies.DataAccess.Data;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    //options.ShouldInclude(new ApiDescription { GroupName = "v1"});

    options.AddDocumentTransformer((document, context, cancelationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.TryAdd("Bearer", new OpenApiSecurityScheme
        {
            Scheme = "Bearer",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
        });

        document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }] = new string[] { }
        });
        return Task.CompletedTask;
    });
});

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true; // Enable reporting of API versions in responses
    options.AssumeDefaultVersionWhenUnspecified = true; // Use default version if not specified
    options.DefaultApiVersion = new ApiVersion(1, 0); // Set default API version
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version"));
})
.AddApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//Controllers
builder.Services.AddTransient<IUserRepos, UserRepos>();
builder.Services.AddTransient<IMovieRepos, MovieRepos>();
builder.Services.AddTransient<IPlaylistRepos, PlaylistRepos>();
builder.Services.AddTransient<IPlaylistMovieRepos, PlaylistMovieRepos>();

//builder.Services.AddAuthentication("Bearer")
//    .AddJwtBearer("Bearer", options =>
//    {
//        //options.Authority = builder.Configuration["AuthorityUrl"] ?? throw new InvalidOperationException("Authority URL not configured.");
//        options.Audience = "MoviesAPI";
//        options.RequireHttpsMetadata = false; // Set to true in production
//    });

var app = builder.Build();

// Versioning and API Explorer setup
var apVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
if(app.Environment.IsDevelopment())
{
    Console.WriteLine("Discovered API Versions.");
    foreach (var description in apVersionDescriptionProvider.ApiVersionDescriptions)
    {
        Console.WriteLine($"API Version: {description.GroupName} - {description.ApiVersion}");
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // Scalar API reference endpoint
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/openapi/v1.json", "Movies API V1");
    //    options.SwaggerEndpoint("/openapi/v2.json", "Movies API V2");
    //    //options.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    //});
}

await using (var serviceScope = app.Services.CreateAsyncScope())
await using (var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

