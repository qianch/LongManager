using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongManager.Port
{
    public class LongSerialPort
    {
        public SerialPort _serialPort = new SerialPort();
        public LongSerialPort()
        {
            _serialPort.BaudRate = 9600;
            _serialPort.StopBits = StopBits.One;
            _serialPort.DataBits = 8;
            _serialPort.Parity = Parity.None;
            _serialPort.ReadTimeout = 8000;
            _serialPort.WriteTimeout = 8000;
            _serialPort.ReadBufferSize = 1024;
            _serialPort.WriteBufferSize = 1024;
            _serialPort.DataReceived += DataReceived;
            _serialPort.Open();
        }

        //接收数据
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

        }

        //发送数据
        public void SendData()
        {

        }
    }
}
