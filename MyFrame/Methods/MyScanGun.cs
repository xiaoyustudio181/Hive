using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace MyFrame
{
    public class MyScanGun
    {
        SerialPort Port;
        public MyScanGun(string PortName, string BaudRate)//记住给扫描枪增加回车符，否则程序将无法读取
        {
            Port = new SerialPort();
            Port.PortName = PortName;
            Port.BaudRate = int.Parse(BaudRate);
            Port.RtsEnable = true;
        }
        public bool Open()
        {
            try
            {
                Port.Open();
                Port.DiscardInBuffer();
                Port.DiscardOutBuffer();
                return true;
            }
            catch (Exception e)
            {
                MyDialog.Msg("打开扫描枪串口失败。\n" + e.Message, 3);
                return false;
            }
        }
        int index;//读取串口时的指定偏移量
        public string GetScanResult()
        {
            if (Port.BytesToRead == 0) return "X";//串口接收缓冲区无数据
            byte[] ScanData = new byte[128];//扫描枪读取的数据
            string Result;//翻译成的字符串（扫描结果）
            index = 0;
            while (true)//逐位读取扫描数据
            {
                Port.Read(ScanData, index, 1);//每次读一位
                if (ScanData[index] == 0x0D //读到回车符\r
                    || index == 127)//已达最大长度，仍未读到\r
                    break;//终止读取

                index++;//偏移量向后移一位
            }
            if (index == 127) return "X";//接收数据超长
            Result = Encoding.ASCII.GetString(ScanData);//翻译扫描结果，将数组转化为字符串
            Result = Result.Substring(0, Result.IndexOf('\r'));//取第一个回车符之前的部分（截掉上次扫描留下的多余部分）
            //if (Result.Contains(";")) Result = Result.Split(';')[2];//若扫描的是模块号，则只取ICCID
            return Result;
        }
        public void Close()
        {
            Port.Close();
        }
    }
}
