using System;
using System.Net.Sockets;
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
        }


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
            Console.WriteLine(taple.bytes.ToString());
        }

        private void testSend_Click(object sender, EventArgs e)
        {
            _sender.Send(Encoding.UTF8.GetBytes("test"));
        }
    }
}