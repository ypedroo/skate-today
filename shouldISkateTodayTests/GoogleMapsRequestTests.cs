using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Refit;
using shouldISkateToday.Clients.RequestInterface;
using shouldISkateToday.Services;

namespace shouldISkateTodayTests;

public class GoogleMapsRequestsTests
{
    private readonly Mock<IGoogleMapsRequests> _requestMock;
    private readonly GoogleMapService _sut;

    public GoogleMapsRequestsTests()
    {
        _requestMock = new Mock<IGoogleMapsRequests>();
        _sut = new GoogleMapService(_requestMock.Object);
    }

    [Fact]
    public async Task ShouldReturnResultFalseOnError()
    {
        _requestMock.Setup(x => x.GetSkateParks(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());


        var result = await _sut.GetSkateParks("skatesparks in fortaleza");

        result.IsSuccess.Should().BeFalse();
    }


    [Fact]
    public async Task ShouldReturnResultSuccessAndEmptyWhenApiDoesntReturn()
    {
        _requestMock.Setup(x => x.GetSkateParks(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new ApiResponse<string>(new HttpResponseMessage(), string.Empty, new RefitSettings()));


        var result = await _sut.GetSkateParks("skatesparks in fortaleza");

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldRespondWithUnprocessableEntityIfSpotNotPassed()
    {
        var result = await _sut.GetSkateParks(string.Empty);

        result.IsFaulted.Should().BeTrue();
    }
}