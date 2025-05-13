using ActivityService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Token environment'tan alınıyor
var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
var jwtKey = builder.Configuration["Jwt:Key"] ?? "devtrackrsupersecretkey";

// 🔐 JWT Ayarları
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateLifetime = true
        };
    });

// 🔗 GitHub Client
builder.Services.AddHttpClient("GitHubClient", client =>
{
    if (!string.IsNullOrEmpty(githubToken))
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", githubToken);
    }

    client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DevTrackr", "1.0"));
});

// 🔗 UserService Client (GitHub username çekmek için)
builder.Services.AddHttpClient("UserClient", client =>
{
    client.BaseAddress = new Uri("http://userservice-devtrackr:8080");
});

// ActivityService'i Scoped olarak bağla
builder.Services.AddScoped<IActivityService, ActivityService.Services.ActivityService>();
builder.Services.AddHttpContextAccessor();

// Swagger + JWT Auth
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Activity API", Version = "v1" });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT token girin. Örn: Bearer {token}"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication(); // 🔐 Auth middleware aktif olmalı
app.UseAuthorization();

app.MapControllers();
app.Run();
