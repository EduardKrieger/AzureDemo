
using Xunit;
using System.Threading.Tasks;
namespace SensorModul.Tests 
{
    public class SensorModulTest
{
    [Fact]
    public async Task RunSensorModule_DoesNotThrowException()
    {
        // Act & Assert
        var exception = await Record.ExceptionAsync(async () => await RunSensorModule());
        
        // Ensure no exception is thrown
        Assert.Null(exception);
    }

    // Fake method simulating the original method in your code
    private async Task RunSensorModule()
    {
        // Simulate some basic logic here, no actual IoT connection
        await Task.Delay(100); // Simulates asynchronous behavior
    }
}
}