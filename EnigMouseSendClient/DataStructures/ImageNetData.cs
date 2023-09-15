using Microsoft.ML.Data;

namespace ObjectDetection.DataStructures
{
    public class ImageNetData
    {
        [LoadColumn(0)]
        public string ImagePath;

        [LoadColumn(1)]
        public string Label;

        /// <summary>
        /// 画像のパスを渡すとそのパスのImageNetDataを生成して返す
        /// </summary>
        /// <param name="ImagePath"></param>
        /// <returns></returns>
        public static IEnumerable<ImageNetData> ReadFromFile(string ImagePath)
        {
            yield return new ImageNetData { ImagePath = ImagePath, Label = Path.GetFileName(ImagePath) };
        }
    }
}