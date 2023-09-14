using OpenCvSharp;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using UnityEasyNet;
using static EnigMouseSendClient.FilePath;

namespace EnigMouseSendClient
{
    public partial class Form1 : Form
    {

        #region 通信周り
        /// <summary>
        /// 通信の確立を受信するUDPReceiver
        /// </summary>
        private UDPReceiver MasterPC_UDPReceiver;

        /// <summary>
        /// 通信の確立を送信するUDPSender
        /// </summary>
        private UDPSender MasterPC_UDPSender;

        /// <summary>
        /// 画像を受信するUDPReceiver
        /// </summary>
        private UDPReceiver ImageUDPReceiver;
        /// <summary>
        /// 物体検出の結果を送信するUDPSender
        /// </summary>
        private UDPSender ResultUDPSender;

        /// <summary>
        /// 通信の確立を受信するポート番号
        /// </summary>
        public static int CommunicationReceivePort = 12010;

        /// <summary>
        /// 通信の確立を送信するポート番号
        /// </summary>
        public static int CommunicationSendPort = 12011;

        /// <summary>
        /// 画像の受信をするポート番号
        /// </summary>
        public static int ImageReceivePort = 12012;

        /// <summary>
        /// 物体検出の結果を送信するポート番号
        /// </summary>
        public static int ResultSendPort = 12013;

        #endregion

        private string ClientIPAddress = "";
        public Form1()
        {
            InitializeComponent();
            AllocConsole();

            //IPv4のアドレスを取得して表示
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in ipHostEntry.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ClientIPAddress = ip.ToString();
                    break;
                }
            }
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
            //一度のみUDPSenderなどを生成する
            if (MasterPC_UDPReceiver != null) { return; }
            MasterPC_UDPReceiver = new UDPReceiver(CommunicationReceivePort, MasterPC_UDPReceiver_Receiver, MasterPC_UDPReceiver_IPEndpoint);

            //画像取得用のUDPを生成
            ImageUDPReceiver = new UDPReceiver(ImageReceivePort, ObjectDetection);
        }
        private void MasterPC_UDPReceiver_Receiver(byte[] obj)
        {
        }
        private void MasterPC_UDPReceiver_IPEndpoint(IPEndPoint point)
        {
            //MasterPCのIPアドレスをもとにUDPSenderを作成する
            MasterPC_UDPSender = new UDPSender(point.Address, CommunicationSendPort);
            ResultUDPSender = new UDPSender(point.Address, ResultSendPort);

            //通信の確立をMasterPCに宣言
            MasterPC_UDPSender.Send(Encoding.UTF8.GetBytes(ClientIPAddress));
        }


        #region 画像取得～物体検出
        uint saveFileIndex = 0;
        private void ObjectDetection(byte[] bytes)
        {
            Console.WriteLine($"ObjectDetection");
            var image = ByteArrayToImage(bytes);
            var TempImageFilePath = Path.Combine(assetsPath, "TempImage", $"{saveFileIndex}.jpeg");
            image.Save(TempImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            saveFileIndex++;
            //最大枚数を1000枚に制限
            if (1000 <= saveFileIndex) { saveFileIndex = 0; }
        }
        public static Image ByteArrayToImage(byte[] b)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(b);
            return img;
        }
        #endregion
    }
}