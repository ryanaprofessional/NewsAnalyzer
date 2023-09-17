using News.Repositories;
using News.Clients;
using Ai.Clients;
using Ai.Repositories;
using Extensions;

var builder = WebApplication.CreateBuilder(args);
// ----------------------------------------------------------------------------------------------
// **News Namespace**
// Clients
builder.Services.AddTransient<NewsApiClient>();

// Repositories
builder.Services.AddTransient<NewsApiRepository>();
builder.Services.AddTransient<News.Repositories.ArticleRepository>();

// ----------------------------------------------------------------------------------------------
// **Ai Namespace**
// Clients
builder.Services.AddTransient<OpenAiClient>();

// Repositories
builder.Services.AddTransient<Ai.Repositories.ArticleRepository>();
builder.Services.AddTransient<OpenAiRepository>();

// ----------------------------------------------------------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------------------------------------------------------
// AWS Config
builder.UseDynamoDb();

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
