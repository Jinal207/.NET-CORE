using Bank.Api.Data;
using Bank.Api.Middleware;
using Bank.Api.Repository;
using Bank.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers().AddNewtonsoftJson(); // Enable JSON Patch

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IBankRepository, BankRepository>();

// Swagger configuration with JWT support
builder.Services.AddSwaggerGen(options =>
    {
        var jwtSecurityScheme = new OpenApiSecurityScheme
        {
            BearerFormat = "JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            Description = "Enter valid JWT token.",
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme
            }
        };

        options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { jwtSecurityScheme, Array.Empty<string>() }
        });
    });

// Database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Authentication
var jwtKey = builder.Configuration["JwtConfig:Key"];
if (string.IsNullOrWhiteSpace(jwtKey))
    throw new Exception("JwtConfig:Key is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero // optional: disable default 5 min clock skew
    };

    // Helpful logs for debugging token validation
    //options.Events = new JwtBearerEvents
    //{ 
    //    OnMessageReceived = ctx =>
    //    {
    //        Console.WriteLine("Authorization header: " + ctx.Request.Headers["Authorization"].FirstOrDefault());
    //        return Task.CompletedTask;
    //    },
    //    OnAuthenticationFailed = ctx =>
    //    {
    //        Console.WriteLine("Authentication failed: " + ctx.Exception?.Message);
    //        return Task.CompletedTask;
    //    },
    //    OnTokenValidated = ctx =>
    //    {
    //        Console.WriteLine(" Token validated for: " + ctx.Principal?.Identity?.Name);
    //        return Task.CompletedTask;
    //    },
    //    OnChallenge = ctx =>
    //    {
    //        Console.WriteLine($" Challenge: error={ctx.Error}, desc={ctx.ErrorDescription}");
    //        return Task.CompletedTask;
    //    }
    //};

    //options.Events = new JwtBearerEvents
    //{
    //    OnChallenge = context =>
    //    {
    //        context.HandleResponse();
    //        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    //        return context.Response.WriteAsync("500: Unauthorized access");
    //    }
    //};
});

// Add Authorization service
builder.Services.AddAuthorization();

builder.Services.AddScoped<JwtService>();

var app = builder.Build();

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseMiddleware<RequestLoggingMiddleware>();

    //Inline Middleware
    //app.Use(async (context, next) =>
    //{
    //    Console.WriteLine("Before next middleware");
    //    await next.Invoke();
    //    Console.WriteLine("After next middleware");
    //});

    // Optional: custom exception middleware
    // app.UseMiddleware<ExceptionHandlingMiddleware>();
    // or
    // app.UseExceptionHandler("/error");
}
app.UseStaticFiles(); // enables serving files from wwwroot

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
