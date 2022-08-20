using CountryService.Web;
using CountryService.Web.Services;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddSingleton<CountryManagementService>();
var app = builder.Build();
app.MapGrpcReflectionService();
app.MapGrpcService<CountryGrpcService>();
// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.To learn how to create a client, visit: https://go.microsoft.com / fwlink /? linkid = 2086909");
app.Run();