using log4net;
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
        private static readonly ILog _log = LogManager.GetLogger(typeof(LongSerialPort));
        public SerialPort _serialPort = new SerialPort();

        public LongSerialPort(string portName)
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
            _serialPort.ErrorReceived += ErrorReceived;
            _serialPort.PortName = portName;
            _serialPort.Open();
        }

        //出错
        private void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            _log.Error("SerialPort发生错误");
        }

        //接收数据
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var length = _serialPort.BytesToRead;
            var buffer = new byte[length];
            _serialPort.Read(buffer, 0, length);
            var data = new StringBuilder();
            foreach (var b in buffer)
            {
                data.Append(b.ToString("X2"));
                data.Append(" ");
            }
            _serialPort.DiscardInBuffer();
            _log.Info("SerialPort接收到数据" + data.ToString().Trim());
        }

        //发送数据
        public void SendData(string data)
        {
            var array = data.Split(' ');
            var buffer = new byte[16];
            var sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                buffer[i] = Convert.ToByte(array[i], 16);
                sum += buffer[i];
            }
            buffer[15] = Convert.ToByte(sum % 256);
            _serialPort.Write(buffer, 0, buffer.Length);
            _log.Info("SerialPort发送数据" + data);
        }
    }
}
