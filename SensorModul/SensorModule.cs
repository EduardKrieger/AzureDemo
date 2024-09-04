// SensorModule.cs
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Client.Transport.Mqtt;
using Newtonsoft.Json;

class SensorModule
{
    static async Task Main(string[] args)
    {
        string connectionString = "Test";//Environment.GetEnvironmentVariable("AZURE_CONNECTION_STRING");

        //var client = DeviceClient.CreateFromConnectionString(connectionString, TransportType.Mqtt);

        Random rand = new Random();
        while (true)
        {
            var telemetryDataPoint = new
            {
                temperature = rand.Next(20, 30)
            };
            var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
            var message = new Message(System.Text.Encoding.ASCII.GetBytes(messageString));
            //await client.SendEventAsync(message);
            Console.WriteLine($"Sent message: {messageString}");
            await Task.Delay(5000);
        }
    }
}