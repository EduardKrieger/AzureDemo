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
string connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");
// Configure the web server
app.MapGet("/", () => "Starting Sensor Module");

app.MapGet("/sensor-data", () => telemetryDataPoint);

app.RunAsync(); // Start the web server asynchronously

// Sensor module logic
async Task RunSensorModule()
{
    var client = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
    Console.WriteLine(client);
    Random rand = new Random();

    while (true)
    {
        Console.WriteLine("Starting to send Messages");
        var temperature = rand.Next(20, 40) ;
        telemetryDataPoint = new { temperature = temperature };
        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
        var message = new Message(System.Text.Encoding.UTF8.GetBytes(messageString))   {
                    ContentEncoding = "utf-8",
                    ContentType = "application/json",
                };;
        message.Properties.Add("SensorID", "EDSens");
        message.Properties.Add("tempAlert", (temperature > 35 ) ? "true" : "false");
        Console.WriteLine(message);
        //Task task = awaitclient.SendEventAsync(message);
        try{
            await client.SendEventAsync(message);
        }catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");

            }
        //Console.WriteLine(task);
        Console.WriteLine($"Sent message: {messageString}");
        await Task.Delay(5000); // Wait for 5 seconds
    }
}

// Start the sensor module asynchronously
await RunSensorModule();