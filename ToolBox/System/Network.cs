﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;

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

        public static string RemoveLastOctetIPv4(string ip)
        {
            ip = ip.Replace($".{ip.Split('.').Last()}", "");
            return $"{ip}.";
        }
    }
}
