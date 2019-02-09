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
            _log.Info("SerialPort接收到数据");
        }

        //发送数据
        public void SendData()
        {
            _log.Info("SerialPort发送数据");
        }
    }
}
