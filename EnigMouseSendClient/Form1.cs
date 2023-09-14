using OpenCvSharp;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEasyNet;
using static EnigMouseSendClient.FilePath;

namespace EnigMouseSendClient
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// MasterPCとの接続を管理するレシーバー
        /// </summary>
        private TCPReceiver MasterPCReceiver;


        private UDPReceiver ImageUDPReceiver;

        private static int CommunicationReceivePort = 12010;
        private static int ImageReceivePort = 12011;
        private static int ResultSendPort = 12012;

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
            //if (MasterPCReceiver != null) { return; }
            //MasterPCReceiver = new TCPReceiver(CommunicationReceivePort, MasterPC_Connection_Receive, MasterPC_Connection_Response);
            //if (ImageUDPReceiver is not null) { return; }
            ////画像取得用のUDPを生成
            ImageUDPReceiver = new UDPReceiver(ImageReceivePort, ObjectDetection);
        }

        private void MasterPC_Connection_Receive((byte[] bytes, int readCount) tuple)
        {
            var s = Encoding.UTF8.GetString(tuple.bytes, 0, tuple.readCount);
            if (s == "connecting")
            {

            }
                Console.WriteLine(s == "connecting");
        }

        int saveFileIndex = 0;
        private void ObjectDetection(byte[] bytes)
        {
            Console.WriteLine($"ObjectDetection");

            var image = ByteArrayToImage(bytes);
            var TempImageFilePath = Path.Combine(assetsPath, "TempImage", $"{saveFileIndex}.jpeg");
            image.Save(TempImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            saveFileIndex++;
            if (1000 <=saveFileIndex) { saveFileIndex = 0; }
/*            Mat mat = Cv2.ImDecode(bytes, ImreadModes.Color);

            Cv2.ImShow("test", mat);*/
        }

        private byte[] MasterPC_Connection_Response()
        {
            return Encoding.UTF8.GetBytes("connecting");
        }
        public static Image ByteArrayToImage(byte[] b)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(b);
            return img;
        }
        #region test
        TCPSender _sender;
        private void testInitSend_Click(object sender, EventArgs e)
        {
            _sender = new TCPSender(SendIP.Text, CommunicationReceivePort, testReciver);
        }

        private void testReciver((byte[] bytes, int readCount) tuple)
        {
            var s = Encoding.UTF8.GetString(tuple.bytes, 0, tuple.readCount);
            Console.WriteLine(s == "connecting");
        }

        private void testSend_Click(object sender, EventArgs e)
        {
            _sender.Send(Encoding.UTF8.GetBytes("test"));
        }
        #endregion
    }
}