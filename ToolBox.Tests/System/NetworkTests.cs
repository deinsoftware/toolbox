using System;
using Xunit;

namespace ToolBox.System.Tests
{
    public class NetworkTests
    {
        [Fact(Skip="It's System Functionality")]
        public void GetLocalIPv4_WhenCalls_NotImplemented()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void RemoveLastOctetIPv4_WhenCalls_ReturnsIPWithoutLastOctet()
        {
            //Arrange
            string ip = "192.168.21.0";
            //Act
            var result = Network.RemoveLastOctetIPv4(ip);
            //Assert
            Assert.Equal("192.168.21.", result);
        }
    }
}