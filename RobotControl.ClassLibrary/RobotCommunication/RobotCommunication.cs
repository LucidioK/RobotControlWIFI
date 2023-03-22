using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RobotControl.ClassLibrary.RobotCommunication
{
    internal class RobotCommunication : IRobotCommunication
    {
        private string ipAddress;
        private HttpClient httpClient;
        public RobotCommunication(string ipAddress)
        {
            this.ipAddress = ipAddress;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"http://{ipAddress}");
        }

        public string[] PortNames => new string[] { "" };

        public void Dispose()
        {
            httpClient.Dispose();
        }

        public string Read()
        {
            return "";
        }

        public Task<string> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public void SetMotors(int l, int r)
        {
            throw new NotImplementedException();
        }

        public Task SetMotorsAsync(int l, int r)
        {
            throw new NotImplementedException();
        }

        public void Start()
        {

        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public void StopMotors()
        {
            throw new NotImplementedException();
        }

        public Task StopMotorsAsync()
        {
            throw new NotImplementedException();
        }

        public void Write(string s)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(string s)
        {
            throw new NotImplementedException();
        }
    }
}
