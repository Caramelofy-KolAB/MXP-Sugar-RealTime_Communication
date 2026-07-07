Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.MapGet("/", () => "Hej från en transformerad console app!");

app.Run();
