using System;
using Xunit;
using ToolBox;

namespace ToolBox.System.Tests
{
    public class NetworkTests
    {
        public NetworkTests()
        {

        }

        [Fact]
        public void RemoveLastOctetIPv4_WhenCalls_ReturnsIPWithoutLastOctet()
        {
            //Arrange
            string ip = "192.168.21.0";
            //Act
            var result = System.Network.RemoveLastOctetIPv4(ip);
            //Assert
            Assert.Equal("192.168.21.", result);
        }
    }
}