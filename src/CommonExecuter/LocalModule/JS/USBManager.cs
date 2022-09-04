using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace eckumoc
{
    public class USBManager
    {
        private static string JAVA_SCRIPT_SOURCE = "var usb = require('usb');function writeMessage( message ){ console.log('NEW MESSAGE'); console.log(JSON.stringify(message)); console.log('END MESSAGE');}writeMessage({ devices: usb.getDeviceList() });usb.on('attach', function (device) { writeMessage(device); device.open(); const iface = device.interfaces[0]; console.log(iface); });usb.on('dettach', function (device) { writeMessage(device); });";
        private static string JAVA_SCRIPT_APPLICATION_FILE =
            System.IO.Directory.GetCurrentDirectory() + "\\read-usb.js";

        


        public ConcurrentQueue<USBDevice> output = new ConcurrentQueue<USBDevice>();
        public List<USBDevice> devices = new List<USBDevice>();
        private Thread background;

        public USBManager()
        {
            System.IO.File.WriteAllText(JAVA_SCRIPT_APPLICATION_FILE, JAVA_SCRIPT_SOURCE);
        }

        public void Init()
        {
            this.background = new Thread(new ThreadStart(()=> {
                ProcessStartInfo info = new ProcessStartInfo("CMD.exe", "/C node " + JAVA_SCRIPT_APPLICATION_FILE);

                info.RedirectStandardError = true;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(info);

                string message = "";
                bool first = true;
                while (true)
                {
                    string line = process.StandardOutput.ReadLine();
                    if( String.IsNullOrEmpty(line)==false)
                    {
                        if( line == "NEW MESSAGE")
                        {
                            message = "";
                        }
                        else if(line=="END MESSAGE")
                        {
                            if (first == true)
                            {
                                JObject devicesObject = JsonConvert.DeserializeObject<JObject>(message);
                                JArray devicesArray = (JArray)devicesObject["devices"];
                                foreach(JObject deviceItem in devicesArray)
                                {
                                    USBDevice device = JsonConvert.DeserializeObject<USBDevice>(deviceItem.ToString());
                                    devices.Add(device);
                                    System.Console.WriteLine(device);
                                }
                                
                                first = false;
                            }
                            else
                            {
                                USBDevice device = JsonConvert.DeserializeObject<USBDevice>(message);
                                output.Enqueue(device);
                            }
                        }
                        else
                        {
                            message += line + "\n";
                        }
                    }
                    
                }
            }));
            this.background.IsBackground = true;
            this.background.Start();
        }
    }
}
