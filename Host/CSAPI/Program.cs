using CSAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterService(builder.Configuration);
var app = builder.Build();
await app.Configure();
app.Run();


