using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

using Newtonsoft.Json;


namespace TestTask
{

    public class NetworkAdapter: IDisposable, ISerializable
    {
        //declarations
        public enum Mode { In, Out, Error, Undefined }

        //static var-s
        private static byte[] STX = new byte[] { 0x02 };
        private static byte[] ETX = new byte[] { 0x03 };
        private static byte[] ACK = new byte[] { 0x06 };
        private static byte[] NAK = new byte[] { 0x15 };
        

        //fields
        private string Encoding = "ASCII";

        private System.Net.Sockets.TcpClient SocketTcpClient;
        private NetworkStream NetworkStream;              
        private Mode State = Mode.Undefined;
        private byte LineFeed;
        private string Host;
        private int Port;

        public event EventHandler<ByteReadedEventArgs> OnByteReaded = (sender, evt) => { };        
        public class ByteReadedEventArgs : EventArgs
        {
            public byte Data { get; private set; }
            public ByteReadedEventArgs( byte Data ): base()
            {
                this.Data = Data;
            }
        }

        public NetworkAdapter(): this("88.212.241.115", 2013, "koi8r")
        {
        }

        public NetworkAdapter(string Host, int Port )
        {     
            this.Host = Host;
            this.Port = Port;
            this.LineFeed = GetBytes("\n")[0];
        }

  

        public NetworkAdapter(string Host, int Port, string Encoding )  
        {
            this.Encoding = Encoding;
            this.Host = Host;
            this.Port = Port;
            this.LineFeed = GetBytes("\n")[0];
        }

        private byte[] GetBytes(string v)
        {
            return Characters.GetBytes(v);
        }

        public void Dispose()
        {
            if(this.SocketTcpClient != null)
            {                
                this.SocketTcpClient.Dispose();
                this.SocketTcpClient = null;
            }
        }

        private TcpClient GetSocketTcpClient()
        {
            try
            {
                if (this.SocketTcpClient == null)
                    this.SocketTcpClient = new System.Net.Sockets.TcpClient(Host, Port);
                return this.SocketTcpClient;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                switch (ex.SocketErrorCode)
                {
                    case SocketError.ConnectionRefused:
                        //заглушка
                        this.Host = "127.0.0.1";
                        this.Port = 2013;
                        this.Encoding = "koi8r";
                        return GetSocketTcpClient();

                        //this.OnConnectionRefused(Host, Port);
                        //return null;                        
                    default: throw;
                }                
            }
            
        }

        private void OnConnectionRefused(string host, int port)
        {

        }

        private NetworkStream GetNetworkStream()
        {
             
            if (this.NetworkStream == null)
            {
                this.NetworkStream = GetSocketTcpClient().GetStream();
            }
            
            return this.NetworkStream;
        }
        public void WriteLine(string message)
        {
            Console.WriteLine($" => {message}");
            this.State = (this.State == Mode.Undefined) ? Mode.Out : this.State;
            byte[] output = GetBytes(message);            
            this.GetNetworkStream().Write(output, 0, output.Length);
            this.State = Mode.In;
        }
        public string ToText(params byte[] data)
        {
            return Characters.GetText(data);
        }
        public byte[] ToBytes(string text )
        {
            return Characters.GetBytes(text);
        }
        public string ReadLine()
        {
            var data = new List<byte>();
            NetworkStream stream = this.GetNetworkStream();
            while (stream.DataAvailable)
            {
                

                byte next = (byte)stream.ReadByte();
                this.OnByteReaded.Invoke(this, new ByteReadedEventArgs(next));
                data.Add(next);                
            }                        
            string result = ToText(data.ToArray());
            if(string.IsNullOrEmpty(result)==false)
                Console.WriteLine( $" <= {result}");
            return result;
        }
        public void Exchange(string output, Action<string> resolve, Action<Exception> reject, Action<byte> readed=null)
        {           
            this.WriteLine(output);
            try
            {                
                while (State == Mode.In)
                {
                    string text = this.ReadLine();
                    resolve(text);
                }
            }
            catch(System.IO.IOException ex)
            {
                this.State = Mode.Error;
            }
            catch(Exception ex)
            {
                LogError(ex);
                reject(ex);
            }
        }

        private void LogError(Exception ex)
        {                   
            while(ex!=null)
            {                
                if (Environment.UserInteractive)
                {
                    Console.WriteLine($"{ex.TargetSite}{ex.Source} => {ex.Message}");                    
                }
                ex = ex.InnerException;
            }
        }

        public void SetInputMode()=> this.State = Mode.In;
        public void SetOutputMode()=> this.State = Mode.Out;
        public int GetNumber(int next)
        {            
            this.WriteLine(next+"\n");
            Console.WriteLine(this.ReadLine());
            return -1;            
        }
        public int CheckAnswer(int next)
        {
            this.WriteLine(next + "\n");
            Console.WriteLine(this.ReadLine());
            return -1;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
           
        }
    }
    public class Characters
    {
        public static Dictionary<int, string> codes = JsonConvert.DeserializeObject<Dictionary<int, string>>(System.IO.File.ReadAllText(@"D:\koi8r.json"));
        public static Dictionary<string, int> decodes = Reverse(codes);
        public static Dictionary<byte, int> numberDecodes = GetNumberDecodes();
        public static Dictionary<byte, string> numberCodes = GetNumberCodes();


        public static Dictionary<byte, string> GetNumberCodes()
        {
            var result = new Dictionary<byte, string>();
            byte[] data = GetBytes("0123456789");
            if (data.Length != 10)
                throw new Exception("Разберись с кодировкой");
            int n = 0;
            foreach (byte bdata in data)
            {
                result[bdata] = n.ToString();
                n++;
            }
            return result;
        }
        public static Dictionary<byte, int> GetNumberDecodes()
        {
            numberDecodes = new Dictionary<byte, int>();

            byte[] data = GetBytes("0123456789");
            if (data.Length != 10)
                throw new Exception("Разберись с кодировкой");
            int n = 0;
            foreach (byte bdata in data)
            {
                numberDecodes[bdata] = n;

                n++;
            }
            return numberDecodes;
        }

        public static string GetText(params byte[] data)
        {
            return GetText(data, 0, data.Length);
        }
        public static string GetText(byte[] data, int start, int bytes)
        {
            string text = "";
            for (int i = start; i < (start + bytes); i++)
            {
                if (codes.ContainsKey(data[i]))
                {
                    text += codes[data[i]];
                }
                else
                {

                }

            }
            return text;
        }
        public static byte[] GetBytes(string message)
        {
            var result = new List<byte>();
            foreach (var ch in message)
            {
                byte next = (byte)decodes["" + ch];
                result.Add(next);
            }
            //result.Add(10);
            return result.ToArray();
        }
        public static Dictionary<string, int> Reverse(Dictionary<int, string> codes)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            codes.ToList().ForEach(kv => result.Add(kv.Value, kv.Key));
            return result;
        }

        public static object GetTextForMe(params byte[] data)
        {
            string result = "";
            foreach (byte next in data)
            {
                if (next == decodes["\n"]) result += @"\n";
                if (next == decodes["\t"]) result += @"\t";
                if (next == decodes["\r"]) result += @"\r";
                if (next == decodes[" "]) result += @"_";
                else result += codes[next];
            }
            return result;
        }

        public static string GetTextLikeBytes(string result)
        {
            string text = "";
            GetBytes(result).ToList().ForEach(b => text += text.Length == 0 ? b.ToString() : "," + b.ToString());
            return text;
        }

        public static string GetBytesLikeText(byte[] result)
        {
            string text = "";
            result.ToList().ForEach(b => text += text.Length == 0 ? b.ToString() : "," + b.ToString());
            return text;
        }
    }
}
