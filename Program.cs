using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
//using Oracle.ManagedDataAccess.Client;
using ReactMaterialUIShowcase.Data;
using ReactMaterialUIShowcaseApi.Interfaces;
using ReactMaterialUIShowcaseApi.Repositories;
using ReactMaterialUIShowcaseApi.Data;
using ReactMaterialUIShowcaseApi.Helpers;
using ReactMaterialUIShowcaseApi.Models;
using ReactMaterialUIShowcaseApi.Resources;
using ReactMaterialUIShowcaseApi.Services;
using Serilog;
using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Bind AppSettings section
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

// Add services to the container.
builder.Services.AddControllers();

// Add Swagger/OpenAPI with JWT support
// Swagger UI URL: https://localhost:7099/
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ReactMaterialUIShowcaseApi", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"] ?? string.Empty)),
    };
});

// Register ApplicationDBContext with connection string from appsettings.json
//builder.Services.AddDbContext<ApplicationDBContext>(options =>
//    options.UseOracle(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<AuthorizationRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<ISchedulerEventRepository, SchedulerEventRepository>();
builder.Services.AddScoped<ISiteProfileRepository, SiteProfileRepository>();
builder.Services.AddScoped<ISiteSettingsRepository, SiteSettingsRepository>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();

builder.Services.AddMemoryCache();

// Add localization services
builder.Services.AddLocalization();// (options => options.ResourcesPath = "Resources");
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(ErrorMessages));
    });

// Add CORS (Cross-Origin Resource Sharing)
// Change the allowed origins for other environments as needed
string[]? allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(allowedOrigins ?? Array.Empty<string>())
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Customize API behavior to suppress automatic model state validation
// We will handle model state validation manually in the controllers to provide localized error messages
// because the language is not known at the time of automatic validation.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Add localization middleware
app.UseRequestLocalization(options =>
{
    var supportedCultures = new[] { "en", "fr" };
    options.SetDefaultCulture("en")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Use Swagger in all environments
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReactMaterialUIShowcaseApi v1");
        // Swagger UI is served at the root URL of your application.
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-API-Version", "1.7.503");
    await next();
});

// Global exception handler to avoid try/catch blocks in every endpoint
app.UseExceptionHandler(builder =>
{
    builder.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerFeature>();
        var ex = feature?.Error;

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = ex switch
        {
            //NotFoundException => StatusCodes.Status404NotFound, // custom exception handler, not implemented yet.
            ValidationException => StatusCodes.Status400BadRequest,
            InvalidDataException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        var result = JsonSerializer.Serialize(new
        {
            error = ex?.Message,
            type = ex?.GetType().Name
        });

        await context.Response.WriteAsync(result);
    });
});


//See database
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

    try
    {
        logger.LogInformation("Starting database migration and seeding...");
        SeedData.Initialize(db, logger);
        logger.LogInformation("Database seeding completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while seeding the database.");
    }


}

app.MapControllers();

app.Run();

