using PowerDispense.Interfaces;
using PowerDispense.MockData;
using PowerDispense.Models;
using PowerDispense.Models.Constants;
using PowerDispense.Models.DTO;
using PowerDispense.Models.Enum;
using PowerDispense.Repositories.Power;
using PowerDispense.Services;
using System.Diagnostics.Metrics;

namespace PowerDispense.Tests;

public class AEDCServiceTest
{
    [Fact]
    public async void ValidateMeter_ReturnsMeterInfo()
    {
        // Arrange
        var powerRequest = new PowerRequest()
        {
            AmountPaid = 100,
            MeterNo = "123456",
            MeterProvider = PowerProvider.AEDC,
            PurchaseDate = new DateTime()
        };
        var AEDCService = new AEDCService();

        // Act
        var actual = await AEDCService.ValidateMeter(powerRequest);
        var meterInfo = await new FilePowerRepo().GetMeter(powerRequest);

        var expected = new MeterInquiryResponse();
        expected.PowerProviderInfo = new PowerProviderInfo()
        {
            Status = CacheKey.POWER_PROVIDER_STABLE
        };
        expected.MeterInfo = meterInfo;

        // Assert
        Assert.Equal(expected, actual);
    }
}