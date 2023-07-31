using News.Services;
using News.Repositories;
using News.Clients;
using Ai.Services;
using Ai.Clients;
using Ai.Repositories;

var builder = WebApplication.CreateBuilder(args);
// ----------------------------------------------------------------------------------------------
// **News Namespace**
// Clients
builder.Services.AddTransient<NewsApiClient>();

// Repositories
builder.Services.AddTransient<NewsApiRepository>();
builder.Services.AddTransient<News.Repositories.ArticleRepository>();


// Services
builder.Services.AddTransient<News.Services.ControllerResponseService>();

// ----------------------------------------------------------------------------------------------
// **Ai Namespace**
// Clients
builder.Services.AddTransient<OpenAiClient>();

// Repositories
builder.Services.AddTransient<Ai.Repositories.ArticleRepository>();
builder.Services.AddTransient<OpenAiRepository>();

// Services
builder.Services.AddTransient<Ai.Services.ControllerResponseService>();

// ----------------------------------------------------------------------------------------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
