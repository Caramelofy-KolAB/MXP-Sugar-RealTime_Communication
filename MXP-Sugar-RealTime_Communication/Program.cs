using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder();
builder.Services.AddSignalR();

// App

var app = builder.Build();

app.MapHub<PingPongHub>("/pingpong");
app.MapGet("/", () => "Hello from a transformed console app!");

_ = app.RunAsync();
Console.WriteLine("Server started...");

// Hub connection

var hubConnection = new HubConnectionBuilder()
    .WithUrl("http://localhost:62894/pingpong")
    .Build();

hubConnection.On<string>("ReceivePong", (message) 
    => {
        Console.WriteLine($"[Client] Received from server: {message}");
    });

await hubConnection.StartAsync();
Console.WriteLine("Client connected");
Console.WriteLine("Press 'P' on the keyboard to Ping the server. Press any other key to exit");

while (true)
{
    var key = Console.ReadKey(intercept: true).Key;
    if (key == ConsoleKey.P)
    {
        Console.WriteLine("\n[Client] Sending: Ping");
        await hubConnection.InvokeAsync("SendPing");
    }
    else
    {
        break;
    }
}

public class PingPongHub : Hub
{
    public async Task SendPing() => await Clients.All.SendAsync("ReceivePong", "Pong");
}