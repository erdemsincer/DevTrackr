using ActivityService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Token environment'tan alýnýyor (GITHUB_TOKEN)
var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

// HttpClient + Authorization ayarlarý
builder.Services.AddHttpClient<IActivityService, ActivityService.Services.ActivityService>(client =>
{
    if (!string.IsNullOrEmpty(githubToken))
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("token", githubToken);
    }

    client.DefaultRequestHeaders.UserAgent.Add(
        new ProductInfoHeaderValue("DevTrackr", "1.0"));
});

// Swagger + API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
