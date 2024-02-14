using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1
{
    public class WMDispatcherService : APIBaseService, IDispatcherService
    {
        public WMDispatcherService()//ILoggerFactory loggerFactory)
        {
        }
        private class ResponseMessage
        {
            public string message { get; set; }
        }
        private string tcpPort;
        public void Initialize(string tcpPort)
        {
            this.tcpPort = tcpPort;
        }

        public async Task<string> InvokeService(string messageString)
        {
            ResponseMessage responseMessage = new ResponseMessage() { message = string.Empty };

            //await Task.Delay(1);
            await Task.Factory.StartNew(() =>
            {
                var netMQRuntime = new NetMQRuntime();
                netMQRuntime.Run(RunClient(messageString, responseMessage)); netMQRuntime.Dispose();
            });
            return responseMessage.message;

        }
        async Task RunClient(string messageString, ResponseMessage responseMessage)
        {

            using (var client = new DealerSocket())
            {

                bool more = false;
                try
                {
                    _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - In");
                    //string address ="tcp://51.140.43.94";
                    //string address ="tcp://192.168.1.76";
                    string address = "tcp://127.0.0.1";
                    string bindingAddress = $"{address}:{tcpPort}";
                    client.Connect(bindingAddress);
                    _logger?.Info("Connected to Address: {0} at {1}", bindingAddress, DateTime.Now.ToLongTimeString());
                    client.SendFrame(messageString);
                    _logger?.Info("Request Send: {0} at {1}", messageString, DateTime.Now.ToLongTimeString());
                    (responseMessage.message, more) = await client.ReceiveFrameStringAsync();
                    _logger?.Info("Request Received: {0} at {1}", responseMessage.message, DateTime.Now.ToLongTimeString());
                }
                catch (Exception ex)
                {
                    responseMessage.message = ex.ToString();
                    _logger?.Error("Request error: {0} at {1}", responseMessage.message, DateTime.Now.ToLongTimeString());
                }
                finally
                {
                    _logger?.Info($"{GetType().ToString()}::{MethodBase.GetCurrentMethod().Name} - Out");
                }
            }
        }

    }
}
