using MessagePack;
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
            imageRecognition = new ImageRecognition();

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
            PCIPAddress.Text = ClientIPAddress;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
        }



        private void MasterPC_Connection_Click(object sender, EventArgs e)
        {
            //一度のみUDPSenderなどを生成する
            if (MasterPC_UDPReceiver != null) { return; }
            isConnected.Text = "Waiting for connection";
            MasterPC_UDPReceiver = new UDPReceiver(CommunicationReceivePort, MasterPC_UDPReceiver_Receiver, MasterPC_UDPReceiver_IPEndpoint);

            //画像取得用のUDPを生成
            ImageUDPReceiver = new UDPReceiver(ImageReceivePort, ObjectDetection);
        }
        private void MasterPC_UDPReceiver_Receiver(byte[] bytes)
        {
            isConnected.Text = "Connected";
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
        bool isBusy = false;
        private async void ObjectDetection(byte[] bytes)
        {
            var image = ByteArrayToImage(bytes);
            var TempImageFilePath = Path.Combine(assetsPath, "TempImage", $"{saveFileIndex}.jpeg");
            image.Save(TempImageFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            saveFileIndex++;
            //最大枚数を1000枚に制限
            if (1000 <= saveFileIndex) { saveFileIndex = 0; }

            //TODO:物体検出の処理に置き換え

            List<ResultStruct> result;
            result = await Task.Run(() => ImageRecognition(TempImageFilePath));

            var masterPCResult = new MasterPCResultStruct(ClientIPAddress, result);
            byte[] resultBytes = MessagePackSerializer.Serialize(masterPCResult);

            ResultUDPSender.Send(resultBytes);
        }
        public static Image ByteArrayToImage(byte[] bytes)
        {
            ImageConverter imgconv = new ImageConverter();
            Image img = (Image)imgconv.ConvertFrom(bytes);
            return img;
        }

        #region 物体検出関数

        ImageRecognition imageRecognition;

        /// <summary>
        /// 引数のパスに存在する画像を画像認識にかける関数
        /// </summary>
        /// <param name="TempImageFilePath"></param>
        private List<ResultStruct> ImageRecognition(string TempImageFilePath)
        {
            List<ResultStruct> results = imageRecognition.ImageRecognitionToFilePath(TempImageFilePath);

            //結果を表示
            //初期化
            Cross_X.Text = "";
            Cross_Y.Text = "";
            Cross_Accuracy.Text = "";

            Dot_X.Text = "";
            Dot_Y.Text = "";
            Dot_Accuracy.Text = "";

            Round_X.Text = "";
            Round_Y.Text = "";
            Round_Accuracy.Text = "";

            Line_X.Text = "";
            Line_Y.Text = "";
            Line_Accuracy.Text = "";

            foreach (ResultStruct resultStruct in results)
            {

                switch (resultStruct.Label)
                {
                    case "Cross":
                        {
                            Cross_X.Text = resultStruct.PosX.ToString();
                            Cross_Y.Text = resultStruct.PosY.ToString();
                            Cross_Accuracy.Text = resultStruct.Confidence.ToString();
                        }
                        break;
                    case "Dot":
                        {
                            Dot_X.Text = resultStruct.PosX.ToString();
                            Dot_Y.Text = resultStruct.PosY.ToString();
                            Dot_Accuracy.Text = resultStruct.Confidence.ToString();
                        }
                        break;
                    case "Line":
                        {
                            Round_X.Text = resultStruct.PosX.ToString();
                            Round_Y.Text = resultStruct.PosY.ToString();
                            Round_Accuracy.Text = resultStruct.Confidence.ToString();
                        }
                        break;
                    case "Round":
                        {
                            Line_X.Text = resultStruct.PosX.ToString();
                            Line_Y.Text = resultStruct.PosY.ToString();
                            Line_Accuracy.Text = resultStruct.Confidence.ToString();
                        }
                        break;
                }
            }

            return results;
        }
        #endregion
        #endregion
    }
}