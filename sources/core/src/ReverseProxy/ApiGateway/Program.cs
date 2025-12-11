using ApiGateway.Attributes;
using ApiGateway.DepencencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddJwtAuthenticationApiGateway(builder.Configuration);
builder.Services.AddReverseProxyApiGateway(builder.Configuration);
builder.Services.AddScoped<CustomJwtBearerEvents>();
builder.Services.AddRedisAuthorizationApi(builder.Configuration);
builder.Services.AddServicesAuthorizationApi();

var app = builder.Build();

app.UseRouting();
app.UseCors("AllowFrontend");

//app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
