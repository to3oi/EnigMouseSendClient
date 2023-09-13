using System;
using System.CodeDom;
using System.Collections;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using UnityEasyNet;


namespace EnigMouseSendClient
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// MasterPCとの接続を管理するレシーバー
        /// </summary>
        private TCPReceiver MasterPCReceiver;

        private static int CommunicationReceivePort = 12010;
        private static int ImageReceivePort = 12010;
        private static int ResultSendPort = 12010;

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
        }
        //デバッグ用
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        //デバッグ用
        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole();

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void MasterPC_Connection_Click(object sender, EventArgs e)
        {
            MasterPCReceiver = new TCPReceiver(CommunicationReceivePort, MasterPC_Connection_Receive, MasterPC_Connection_Response);
        }

        private void MasterPC_Connection_Receive((byte[] buffer, int readCount) tuple)
        {

        }

        private byte[] MasterPC_Connection_Response()
        {
            return Encoding.UTF8.GetBytes("connecting");
        }

        TCPSender _sender;
        private void testInitSend_Click(object sender, EventArgs e)
        {
            _sender = new TCPSender(SendIP.Text, CommunicationReceivePort, testReciver);
        }

        private void testReciver((byte[] bytes, int readCount) taple)
        {
            var s = Encoding.UTF8.GetString(taple.bytes,0,taple.readCount);
            Console.WriteLine(s == "connecting");
        }

        private void testSend_Click(object sender, EventArgs e)
        {
            _sender.Send(Encoding.UTF8.GetBytes("test"));
        }
    }
}