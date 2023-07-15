using NewsAnalyzer.Services;
using NewsAnalyzer.Repositories;
using NewsAnalyzer.Clients;

var builder = WebApplication.CreateBuilder(args);

// Clients
builder.Services.AddTransient<NewsApiClient>();

// Repositories
builder.Services.AddTransient<NewsApiRepository>();

// Services
builder.Services.AddTransient<QueryNewsApiService>();
builder.Services.AddTransient<ControllerResponseService>();

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
