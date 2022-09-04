using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using COM;

using Newtonsoft.Json;


namespace MessageLevel
{
    public class MessageClient: CharacterEncoder
    { 
        private TcpClient Client;
        private NetworkStream Stream;
        //private int Rate = 4;

        public MessageClient(string Host, int Port)
        {
            this.Client = new TcpClient(Host, Port);
            this.Stream = Client.GetStream();
        }

        public string Request(string message)
        {
            
            this.Write(message);
            var commands = this.Read();
            string text = "";
            foreach(var line in commands.ToArray())
            {
                text += line + "\n";
            }
            return text;
        }

        private void Write(string message)
        {
            Write(message.ToCharArray().Select(ch => (byte)ch).ToArray());            
        }

        private void Write(byte[] data)
        {
            Stream.Write(data);
        }

        public TextMessage Request(TextMessage message  )
        {
            
            
            this.Write(message);
            message = this.Read();
      

            return message;
        }


        IDictionary<byte, char> encoding = new Dictionary<byte, char>();


        private TextMessage Read( params byte[] sep )
        {
            TextMessage text = new TextMessage();
            while (Stream.CanRead)
            {
                StringBuilder builder = new StringBuilder();
                byte[] buffer = new byte[1024 * 10];
                int readed = Stream.Read(buffer, 0, buffer.Length);
                if (readed < 0)
                    throw new Exception("Чтение завершено");
                var package = new byte[readed];
                text.Enqueue(package);
            }
            return text;
        }

        private void Write(TextMessage message)
        {
            foreach(var line in message)
            {
                Stream.Write(line, 0, line.Length);              
            }
        }

        public NetworkStream GetNetworkStream() => this.Stream;
    }
}
