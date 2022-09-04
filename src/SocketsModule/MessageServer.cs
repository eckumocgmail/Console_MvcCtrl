using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using ServiceEndpoint.ServiceEndpoint;

using ValidationAnnotationsNS;

namespace MessageLevel
{
    public abstract class MessageServer: MyValidatableObject, IDisposable
    {
        

        private NetworkStream NetworkStream;
        private string IP;
        private int Port;

        public TcpListener TcpListener { get; }
        public Thread Thread { get; private set; }

        public MessageServer() : this("127.0.0.1", 8181)
        { 
        }

        public MessageServer(string ip, int port) 
        {
            this.IP = ip;
            this.Port = port;
            this.TcpListener = new TcpListener(IPAddress.Parse(ip), port);
        }





        public NetworkStream GetNetworkStream() => NetworkStream;
        public void Start()
        {
            this.EnsureIsValide();

            TextMessage message = new TextMessage();
            this.Thread = new Thread(() => {
                TcpListener.Start();
                TcpClient Client = TcpListener.AcceptTcpClient();
                using (NetworkStream = Client.GetStream())
                {
                    while (true)
                    {
                        try
                        {
                            var message = Read();
                            //message = OnTextMessage(message);
                            Write(message);

                        } catch (Exception ex)
                        {
                            Error(ex);
                        }
                    }

                }
                
            });
            this.Thread.Name = "t-"+GetHashCode();
            this.Thread.IsBackground = true;
            this.Thread.Start();
            
        }

        private void Write(object message)
        {
            throw new NotImplementedException();
        }

        private object Read()
        {
            throw new NotImplementedException();
        }

         
    }
}
