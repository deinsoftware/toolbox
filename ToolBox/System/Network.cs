using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using ToolBox.Validations;

namespace ToolBox.System
{
    public static class Network
    {
        public static string GetLocalIPv4()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }

        public static string GetOctetsIPv4(string ip, int amount)
        {
            if (!Number.IsOnRange(1, amount, 4))
            {
                throw new ArgumentException("Must be a number beteween 1 and 4.", nameof(amount));
            }

            string[] octets = ip.Split('.');
            var result = new StringBuilder();
            for (var i = 0; i < amount; i++)
            {
                result.Append(octets[i]);
                if (i < 3)
                {
                    result.Append(".");
                }
            }
            return result.ToString();
        }
    }
}
