using Microsoft.EntityFrameworkCore;
using url_shortener_api.Context;
using url_shortener_api.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<URLContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("URLContext")));

builder.Services.AddScoped<UrlShorteningService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors(builder => builder
   .SetIsOriginAllowed(_ => true)
   .AllowAnyMethod()
   .AllowAnyHeader()
   .AllowCredentials());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
