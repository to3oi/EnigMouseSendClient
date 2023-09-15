using ObjectDetection.CustomVisionParser;
using ObjectDetection.DataStructures;
using ObjectDetection;
using Microsoft.ML;
using static EnigMouseSendClient.FilePath;
namespace EnigMouseSendClient
{
    /// <summary>
    /// 画像認識を実行するためのクラス
    /// </summary>
    public class ImageRecognition
    {
        public ImageRecognition() { }


        public List<ResultStruct> ImageRecognitionToFilePath(string _imageFilePath)
        {
            // Initialize MLContext
            MLContext mlContext = new MLContext();
            List<ResultStruct> results = new List<ResultStruct>();

            try
            {
                // Load Data
                IEnumerable<ImageNetData> images = ImageNetData.ReadFromFile(_imageFilePath);
                IDataView imageDataView = mlContext.Data.LoadFromEnumerable(images);

                // Create instance of model scorer
                var modelScorer = new OnnxModelScorer(modelFilePath, mlContext);

                // Use model to score data
                IEnumerable<float[]> probabilities = modelScorer.Score(imageDataView);

                // Post-process model output
                CustomVisionOutputParser parser = new CustomVisionOutputParser();

                var boundingBoxes =
                    probabilities
                    .Select(probability => parser.ParseOutputs(probability))
                    //.5Fが表示するスコアの値
                    .Select(boxes => parser.FilterBoundingBoxes(boxes, 5, .5F));

                //基本的にimages.Countは1のはずなので複数回回す意味はないが2以上になったときに最後に処理した(indexが大きい最新のもの)の値を返すのに使用
                for (var i = 0; i < images.Count(); i++)
                {
                    string imageFileName = images.ElementAt(i).Label;
                    IList<CustomVisionBoundingBox> detectedObjects = boundingBoxes.ElementAt(i);
                    results = CalculateBoundingBox(detectedObjects);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return results;

        }

        //KinectのDepth,IRのサイズ(固定値)
        int originalImageHeight = 576;
        int originalImageWidth = 640;
        /*
            +---------------------- x
            | 
            | 
            | 
            | 
            | 
            | 
            |
            y
         */
        List<ResultStruct> CalculateBoundingBox(IList<CustomVisionBoundingBox> filteredBoundingBoxes)
        {

            List<ResultStruct> results = new List<ResultStruct>();
            foreach (var box in filteredBoundingBoxes)
            {
                // Get Bounding Box Dimensions
                var x = (uint)Math.Max(box.Dimensions.X, 0);
                var y = (uint)Math.Max(box.Dimensions.Y, 0);
                var width = (uint)Math.Min(originalImageWidth - x, box.Dimensions.Width);
                var height = (uint)Math.Min(originalImageHeight - y, box.Dimensions.Height);

                // Resize To Image
                x = (uint)originalImageWidth * x / OnnxModelScorer.ImageNetSettings.imageWidth;
                y = (uint)originalImageHeight * y / OnnxModelScorer.ImageNetSettings.imageHeight;
                width = (uint)originalImageWidth * width / OnnxModelScorer.ImageNetSettings.imageWidth;
                height = (uint)originalImageHeight * height / OnnxModelScorer.ImageNetSettings.imageHeight;


                //認識した座標を出力
                uint posX = x + width / 2;
                uint posY = y + height / 2;

                /*double resPosX = posX;
                double resPosY = posY;

                //Maskで調整した値を反映
                // X
                int rightMask = form1.GetRightMask;
                int leftMask = form1.GetLeftMask;

                if (posX <= leftMask ||
                 originalImageWidth - rightMask <= posX)
                {
                    //右のMaskの範囲を越していたらresultsに追加しないで終了
                    continue;
                }
                resPosX = (double)(posX - leftMask) * (double)(originalImageWidth / (double)(originalImageWidth - (double)(rightMask + leftMask)));



                // Y
                int topMask = form1.GetTopMask;
                int bottomMask = form1.GetBottomMask;
                if (posY <= topMask ||
                 originalImageHeight - bottomMask <= posY)
                {
                    //下のMaskの範囲を越していたらresultsに追加しないで終了
                    continue;
                }
                resPosY = (double)(posY - topMask) * (double)(originalImageHeight / (double)(originalImageHeight - (double)(topMask + bottomMask)));

                resPosX = resPosX + form1.GetPositionOffsetX;
                resPosY = resPosY + form1.GetPositionOffsetY;

                //UnityのXY軸に補正する

                resPosY = Math.Abs(resPosY - originalImageHeight);*/

                results.Add(new ResultStruct(box.Label, (float)posX, (float)posY, box.Confidence));
            }

            return results;
        }
    }
}