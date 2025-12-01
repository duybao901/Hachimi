using ApiGateway.DepencencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddJwtAuthenticationApiGateway(builder.Configuration);
builder.Services.AddReverseProxyApiGateway(builder.Configuration);

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapReverseProxy();

app.Run();
