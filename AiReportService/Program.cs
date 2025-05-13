using AiReportService.Consumers;
using AiReportService.Data;
using AiReportService.External;
using AiReportService.Jobs;
using AiReportService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using System.Net.Http.Headers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<UserRegisteredConsumer>();
    x.AddConsumer<TaskCompletedConsumer>();              // ✅ ekledik
    x.AddConsumer<PomodoroCompletedConsumer>();          // ✅ ekledik

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("rabbitmq-devtrackr", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("user-registered-event", e =>
        {
            e.ConfigureConsumer<UserRegisteredConsumer>(ctx);
        });

        cfg.ReceiveEndpoint("task-completed-event", e =>
        {
            e.ConfigureConsumer<TaskCompletedConsumer>(ctx);  // ✅ yeni
        });

        cfg.ReceiveEndpoint("pomodoro-completed-event", e =>
        {
            e.ConfigureConsumer<PomodoroCompletedConsumer>(ctx);  // ✅ yeni
        });
    });
});


// JWT Ayarları
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "supersecret");

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// HttpContextAccessor (Token alabilmek için)
builder.Services.AddHttpContextAccessor();

// HttpClient DI Bağlantıları
builder.Services.AddHttpClient<IActivityClient, ActivityClient>(c =>
{
    c.BaseAddress = new Uri("http://activityservice-devtrackr:8080");
});
builder.Services.AddHttpClient<ITaskClient, TaskClient>(c =>
{
    c.BaseAddress = new Uri("http://taskservice-devtrackr:8080");
});
builder.Services.AddHttpClient<IPomodoroClient, PomodoroClient>(c =>
{
    c.BaseAddress = new Uri("http://pomodoroservice-devtrackr:8080");
});
builder.Services.AddHttpClient<IUserClient, UserClient>(c =>
{
    c.BaseAddress = new Uri("http://userservice-devtrackr:8080");
});

// OpenAI Servisi
builder.Services.AddHttpClient<OpenAiService>(client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/");
    var apiKey = builder.Configuration["OpenAI:ApiKey"];
    if (!string.IsNullOrEmpty(apiKey))
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
    }
});

// Uygulama Servisleri
builder.Services.AddScoped<IAiReportService, AiReportService.Services.AiReportService>();

// Quartz Zamanlayıcı: Her Pazar 03:00'te çalışır
builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("WeeklyReportJob");

    q.AddJob<GenerateWeeklyReportsJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("WeeklyReportJob-trigger")
        .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Sunday, 3, 0)));
});

builder.Services.AddQuartzHostedService();

// DbContext
builder.Services.AddDbContext<AiReportDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AI Report API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT token girin. Örn: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddControllers();

// Pipeline
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
