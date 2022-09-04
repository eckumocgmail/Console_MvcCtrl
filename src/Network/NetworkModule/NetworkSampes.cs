using MessageLevel;

using ServiceEndpoint.ServiceEndpoint;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eckumoc_common_api.CommonModule.NetworkModuleTest
{
    public class NetworkSampes
    {

        static void TastingTcp()
        {

            var handler = new TextMessageHandler();


            using (var server = new AppServer("127.0.0.1", 8888))
            {
                server.Start();
                Console.WriteLine("Waiting for connections");
                Thread.Sleep(Timeout.Infinite);


                var client = new AppClient("127.0.0.1", 8888);
                while (true)
                {
                    var message = new TextMessage();
                    message.Enqueue("Greetings\n".ToCharArray().Select(ch => (byte)ch).ToArray());
                    client.Request(message);

                    Thread.Sleep(100);
                }
            }
        }
        public static void Sample()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    System.Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    System.Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        System.Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        System.Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                System.Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            System.Console.WriteLine("\nHit enter to continue...");
            System.Console.Read();
        }

    }
}
