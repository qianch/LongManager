using log4net;
using LongManager.Core;
using LongManager.Core.DataBase;
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
        private LongDbContext _longDBContext = new LongDbContext();
        private SerialPort _serialPort = new SerialPort();

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
            if (length == 0)
            {
                return;
            }

            var buffer = new byte[length];
            _serialPort.Read(buffer, 0, length);
            var data = String.Join(" ", buffer.Select(x => x.ToString("X2")).ToList());
            _serialPort.DiscardInBuffer();
            _log.Info("SerialPort接收到数据" + data);

            var prefixBuffer = buffer.Take(buffer.Length - 1).ToList();
            var sum = prefixBuffer.Sum(x => Convert.ToInt16(x));
            var crc = sum % 256;
            var last = buffer.Last();

            if (crc != last)
            {
                _log.Info("PLC返回的数据CRC校验不通过:" + data);
                return;
            }
            Handle(data);
        }

        //数据处理
        public void Handle(string data)
        {
            var array = data.Split(' ');
            if (array.Length > 1)
            {
                var flag = array[1];
                switch (flag)
                {
                    case "01": //登记车号
                        HandleCarNO(data);
                        break;
                    case "02"://发送电机与标签的关联信息返回结果
                        break;
                    case "03"://登记标签卡号
                        HandleLabelNO(data);
                        break;
                    case "04"://电机测试运行
                        _log.Info("电机测试运行的返回值：" + data);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 保存小车号
        /// </summary>
        /// <param name="data"></param>
        public void HandleCarNO(string data)
        {
            var carNOList = data.Split(' ').Skip(2).Take(7).ToList();
            var carNO = string.Join(" ", carNOList);
            if (_longDBContext.Cars.Where(x => x.CarNO == carNO).Count() == 0)
            {
                var car = new CarBasicInfo()
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    CarNO = carNO
                };

                _longDBContext.Cars.Add(car);
                _longDBContext.SaveChanges();
            }
        }

        /// <summary>
        /// 保存标签号
        /// </summary>
        /// <param name="data"></param>
        public void HandleLabelNO(string data)
        {
            var labelNOList = data.Split(' ').Skip(2).Take(4).ToList();
            var labelNO = string.Join(" ", labelNOList);
            if (_longDBContext.Labels.Where(x => x.LabelNO == labelNO).Count() == 0)
            {
                var label = new LabelBasicInfo()
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    LabelNO = labelNO
                };

                _longDBContext.Labels.Add(label);
                _longDBContext.SaveChanges();
            }
        }

        /// <summary>
        /// 发送电机与标签的关联信息
        /// </summary>
        /// <param name="carNO">小车编号</param>
        /// <param name="labelNO">标签编号</param>
        /// <param name="delayTime">读到标签后延时时间（单位S）</param>
        /// <param name="actionTime">电机动作时间（单位S）</param>
        public void SendOrderData(string carNO, string labelNO, string delayTime = "05", string actionTime = "05")
        {
            var data = new List<string> { "FE", "02" };
            var carNOList = carNO.Split(' ');
            if (carNOList.Count() != 7)
            {
                throw new ArgumentException("车辆编号不正确");
            }
            var labelNOList = labelNO.Split(' ');
            if (labelNOList.Count() != 4)
            {
                throw new ArgumentException("标签编号不正确");
            }
            data.AddRange(carNOList);
            data.AddRange(labelNOList);
            data.Add(delayTime);
            data.Add(actionTime);
            SendData(String.Join(" ", data));
        }

        /// <summary>
        /// 只发送电机运行信息
        /// </summary>
        /// <param name="carNO">小车编号</param>
        /// <param name="delayTime">读到标签后延时时间（单位S）</param>
        /// <param name="actionTime">电机动作时间（单位S）</param>
        public void SendMotorData(string carNO, string delayTime = "05", string actionTime = "05")
        {
            var data = new List<string> { "FE", "04" };
            var carNOList = carNO.Split(' ');
            if (carNOList.Count() != 7)
            {
                throw new ArgumentException("车辆编号不正确");
            }
            data.AddRange(carNOList);
            data.Add(delayTime);
            data.Add(actionTime);
            SendData(String.Join(" ", data));
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="data"></param>
        public void SendData(string data)
        {
            var array = data.Split(' ');
            var bytes = array.Select(x => Convert.ToByte(x, 16)).ToList();
            var sum = bytes.Sum(x => Convert.ToInt16(x));
            bytes.Add(Convert.ToByte(sum % 256));
            var buffer = bytes.ToArray();
            _serialPort.Write(buffer, 0, buffer.Length);
            _log.Info("SerialPort发送数据" + data);
        }
    }
}
