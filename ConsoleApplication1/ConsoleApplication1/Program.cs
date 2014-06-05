using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace ConsoleApplication1
{
    class Program
    {
        public static void Main()
        {
            SerialPort mySerialPort = new SerialPort("COM3");

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
        }

        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();

            Task.Factory.StartNew(() =>
                {
                    using (var theClient = new WebClient())
                    {
                        try
                        {
                            theClient.Headers.Add("Content-Type", "application/json");
                            theClient.UploadString(@"http://localhost:4382/api/data", "POST", "\"" + indata + "\"");
                        }
                        catch (Exception ex)
                        { }

                    }
                });

            Console.WriteLine("Data Received:" + indata);
        }
    }
}
