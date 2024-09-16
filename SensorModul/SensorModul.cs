
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5753");
var app = builder.Build();


var telemetryDataPoint = new { temperature = 0};
//var telemetryDataPoint = new { temperature = 0 , humidity = 0};
string connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");

app.MapGet("/", () => "Starting Sensor Module");

app.MapGet("/sensor-data", () => telemetryDataPoint);

app.RunAsync(); 


async Task RunSensorModule()
{
    var client = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);
    Console.WriteLine(client);
    Random rand = new Random();

    while (true)
    {
        Console.WriteLine("Starting to send Messages");
        var temperature = rand.Next(20, 40) ;
        var humidity = rand.Next(35, 99);
        //telemetryDataPoint = new { temperature = temperature };
        telemetryDataPoint = new { temperature = temperature, humidity= humidity };
        var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
        var message = new Message(System.Text.Encoding.UTF8.GetBytes(messageString))   {
                    ContentEncoding = "utf-8",
                    ContentType = "application/json",
                };;
        message.Properties.Add("SensorID", "TS123");
        //message.Properties.Add("tempAlert", (temperature > 35 ) ? "true" : "false");
        try{
            await client.SendEventAsync(message);
        }catch (Exception ex)
            {
                Console.WriteLine($"Error sending message: {ex.Message}");

            }

        Console.WriteLine($"Sent message: {messageString}");
        await Task.Delay(5000); 
}
}
// Start the sensor module asynchronously
await RunSensorModule();
