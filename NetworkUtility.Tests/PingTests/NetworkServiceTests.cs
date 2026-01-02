using FluentAssertions;
using FluentAssertions.Extensions;
using NetworkUtility.Ping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;


namespace NetworkUtility.Tests.PingTests
{
    public class NetworkServiceTests
    {
        private readonly NetworkService _service;

        public NetworkServiceTests()
        {
            _service = new NetworkService();
        }
        [Fact]
        public void NetworkService_SendPing_ReturnString()
        {
            //Arrange
            //var pingService = new NetworkService();

            //Act
            var result = _service.SendPing();

            // Assert
            // Use the FluentAssertion website to check all kind of assertions:
            // https://fluentassertions.com/introduction
            
            result.Should().NotBeNullOrWhiteSpace();
            result.Should().Be("Success: Ping Sent!");
            result.Should().Contain("Success", Exactly.Once());

        }

        //Theory is used when the unit test function need to receive params
        //Inline is used as an input and return value
        [Theory]
        [InlineData(1,1,2)]
        [InlineData(2,4,6)]
        public void NetworkService_PingTimeout_ReturnInt(int a, int b, int expected)
        {
            //Arrange 
            //var pingService = new NetworkService();
            
            //Act
            var result = _service.PingTimeout(a, b);

            //Assert
            result.Should().Be(expected);
            result.Should().BeGreaterThanOrEqualTo(2);
            result.Should().NotBeInRange(-10000, 0);
        }

        [Fact]
        public void NetworkService_LastPingDate_ReturnDateTime()
        {
            //Arrange
            var pingService = new NetworkService();

            //Act
            var result = _service.LastPingDate();

            // Assert
            // Use the FluentAssertion website to check all kind of assertions:
            // https://fluentassertions.com/introduction

            result.Should().BeAfter(3.January(2010));
            result.Should().BeBefore(3.December(2028));

        }

        [Fact]
        public void NetworkService_GetPingOptions_ReturnsObject()
        {
            //Arrange
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _service.GetPingOptions();

            // Assert: WARNING: Use BeEquivalent when comparing object. string and int works without Equivalent though like previous methods.
            result.Should().BeOfType<PingOptions>();
            result.Should().BeEquivalentTo(expected);
            result.Ttl.Should().Be(1);
        }

        [Fact]
        public void NetworkService_MostRecentPings_ReturnsObject()
        {
            //Arrange
            var expected = new PingOptions()
            {
                DontFragment = true,
                Ttl = 1
            };

            //Act
            var result = _service.MostRecentPings();

            // Assert: WARNING: Use BeEquivalent when comparing object. string and int works without Equivalent though like previous methods.
            //result.Should().BeOfType<IEnumerable<PingOptions>>();
            result.Should().ContainEquivalentOf(expected);
            result.Should().Contain(x => x.DontFragment == true);
        }
    }
}
