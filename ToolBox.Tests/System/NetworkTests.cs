using System;
using Xunit;

namespace ToolBox.System.Tests
{
    public class NetworkTests
    {
        [Fact(Skip = "It's System Functionality")]
        public void GetLocalIPv4_WhenCalls_NotImplemented()
        {
            throw new NotImplementedException();
        }

        [Theory]
        [InlineData("192.", "192.168.21.0", 1)]
        [InlineData("192.168.", "192.168.21.0", 2)]
        [InlineData("192.168.21.", "192.168.21.0", 3)]
        [InlineData("192.168.21.0", "192.168.21.0", 4)]
        public void GetOctetsIPv4_WhenCalls_ReturnsOctetsDefined(string expectedResult, string ip, int amount)
        {
            //Arrange
            //Act
            var result = Network.GetOctetsIPv4(ip, amount);
            //Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("192.168.21.0", 0)]
        [InlineData("192.168.21.0", 5)]
        public void GetOctetsIPv4_WhenAmountIsOutOfRange_ReturnsException(string ip, int amount)
        {
            //Arrange
            //Act
            Action result = () => Network.GetOctetsIPv4(ip, amount);
            //Assert
            Assert.Throws<ArgumentException>(result);
        }
    }
}