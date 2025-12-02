using ApiGateway.Attributes;
using ApiGateway.DepencencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthenticationApiGateway(builder.Configuration);
builder.Services.AddReverseProxyApiGateway(builder.Configuration);
builder.Services.AddScoped<CustomJwtBearerEvents>();
builder.Services.AddRedisAuthorizationApi(builder.Configuration);
builder.Services.AddServicesAuthorizationApi();

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
