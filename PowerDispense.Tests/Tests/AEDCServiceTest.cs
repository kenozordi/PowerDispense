using PowerDispense.Interfaces;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.DTO;
using PowerDispense.Services;

namespace PowerDispense.Tests;

public class AEDCServiceTest
{
    [Fact]
    public void ValidateMeter_ReturnsMeterInfo()
    {
        // Arrange
        var powerRequest = new PowerRequest()
        {
            AmountPaid = 100,
            MeterNo = "123456",
            MeterProvider = MeterInfo.MeterProviders.AEDC,
            PurchaseDate = new DateTime()
        };
        var AEDCService = new AEDCService();

        // Act
        var actual = AEDCService.ValidateMeter(powerRequest);
        var expected = MeterInfoSampleData.meterInfos.Where(meter => meter.MeterNo == powerRequest.MeterNo).SingleOrDefault();
        
        // Assert
        Assert.Equal(expected, actual);
    }
}