using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

// Web application setup
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Sensor telemetry data
var telemetryDataPoint = new { temperature = 0 };
//string connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");

// Configure the web server
app.MapGet("/", () => "Starting Sensor Module");

app.MapGet("/sensor-data", () => telemetryDataPoint);

app.RunAsync(); // Start the web server asynchronously

// Sensor module logic
async Task RunSensorModule()
{
    //var client = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
    Random rand = new Random();

    while (true)
    {
        telemetryDataPoint = new { temperature = rand.Next(20, 30) };
        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
        var message = new Message(System.Text.Encoding.ASCII.GetBytes(messageString));
        //await client.SendEventAsync(message);
        Console.WriteLine($"Sent message: {messageString}");
        await Task.Delay(5000); // Wait for 5 seconds
    }
}

// Start the sensor module asynchronously
await RunSensorModule();