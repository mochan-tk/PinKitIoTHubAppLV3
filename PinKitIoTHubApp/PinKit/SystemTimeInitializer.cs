using Microsoft.SPOT;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace GrFamily.Utility
{
    /// <summary>
    /// NTP�T�[�o�[���猻�ݓ������擾����N���X
    /// </summary>
    public static class SystemTimeInitializer
    {
        //private static string DefaultNtpServer = "time.nist.gov";
        private const string DefaultNtpServer = "ntp.nict.jp";
        private const int DefaultTimezoneOffset = 540;
        private static bool completed = false;

        private static string ntpServerUrl;
        private static int currentTimezone;
        /// <summary>
        /// �f�t�H���g�� NTP �T�[�o�[�A�������g���āANTP�T�[�o�[����������擾���ă��[�J��������ݒ肷��
        /// </summary>
        /// <remarks>�f�t�H���g NTP �T�[�o�[�� ntp.nict.jp<br />
        /// �f�t�H���g������ 540�� (+9����)</remarks>
        public static bool InitSystemTime()
        {
             return InitSystemTime(DefaultNtpServer, DefaultTimezoneOffset);
        }

        /// <summary>
        /// NTP �T�[�o�[�A�������w�肵�āA�������擾���ă��[�J��������ݒ肷��
        /// </summary>
        /// <param name="ntpServer">NTP �T�[�o�[</param>
        /// <param name="timezoneOffset">���� (�P�ʁF��)</param>
        /// <remarks>���<br />
        /// http://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c</remarks>
        public static bool InitSystemTime(string ntpServer, int timezoneOffset)
        {
            ntpServerUrl = ntpServer;
            currentTimezone = timezoneOffset;
            var thread = new Thread(UpdateSystemTime);
            thread.Start();
            if (SystemTimeUpdated == null)
            {
                thread.Join();
            }
            return completed;
        }

        private static void UpdateSystemTime()
        {
            string ntpServer = ntpServerUrl;
            int timezoneOffset = currentTimezone;
            try {
                var ep = new IPEndPoint(Dns.GetHostEntry(ntpServer).AddressList[0], 123);

                var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sock.Connect(ep);

                var ntpData = new byte[48];
                ntpData[0] = 0x1b;
                for (var i = 1; i < 48; i++)
                    ntpData[i] = 0;

                sock.Send(ntpData);
                sock.Receive(ntpData);
                sock.Close();

                const int offset = 40;
                ulong intPart = 0;
                for (var i = 0; i <= 3; i++)
                    intPart = 256 * intPart + ntpData[offset + i];

                ulong fractPart = 0;
                for (var i = 4; i <= 7; i++)
                    fractPart = 256 * fractPart + ntpData[offset + i];

                var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
                var dateTime = new DateTime(1900, 1, 1).AddMilliseconds(milliseconds);
                var networkDateTime = dateTime + new TimeSpan(0, timezoneOffset, 0);

                Microsoft.SPOT.Hardware.Utility.SetLocalTime(networkDateTime);
                completed = true;
            }
            catch(Exception ex)
            {
                Debug.Print("SystemTimeInitializer failed.");
                Debug.Print(ex.Message);
            }
            if (SystemTimeUpdated != null)
            {
                SystemTimeUpdated(new SystemTimeUpdatedEventAggs(completed));
            }
        }

        public static event SystemTimeUpdatedHandler SystemTimeUpdated;
        
    }

    public delegate void SystemTimeUpdatedHandler(SystemTimeUpdatedEventAggs args);
    public class SystemTimeUpdatedEventAggs : EventArgs
    {
        public SystemTimeUpdatedEventAggs(bool succeeded)
        {
            
        }
        public  bool IsSucceeded { get; set; }
        
    }
}
