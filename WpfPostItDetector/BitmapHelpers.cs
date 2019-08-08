using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace WpfPostItDetector
{
    static class BitmapHelpers
    {
        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        public static void CopyPixels(this BitmapSource source, PixelColor[,] pixels, int stride, int offset)
        {
            var height = source.PixelHeight;
            var width = source.PixelWidth;
            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, stride, 0);
            int y0 = offset / width;
            int x0 = offset - width * y0;
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    pixels[x + x0, y + y0] = new PixelColor
                    {
                        Blue = pixelBytes[(y * width + x) * 4 + 0],
                        Green = pixelBytes[(y * width + x) * 4 + 1],
                        Red = pixelBytes[(y * width + x) * 4 + 2],
                        Alpha = pixelBytes[(y * width + x) * 4 + 3],
                    };
        }

        public static double GePrediction(this BitmapSource source)
        {
            double result = 0;

            /*
             * If you have an image URL:
                https://eastus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/204f0dbd-4f4c-47ed-9382-120de54c81b5/classify/iterations/Iteration2/url
                Set Prediction-Key Header to : a8435b876e914433aba85a3f8793a28a
                Set Content-Type Header to : application/json
                Set Body to : {"Url": "https://example.com/image.png"}
                If you have an image file:
                https://eastus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/204f0dbd-4f4c-47ed-9382-120de54c81b5/classify/iterations/Iteration2/image
                Set Prediction-Key Header to : a8435b876e914433aba85a3f8793a28a
                Set Content-Type Header to : application/octet-stream
                Set Body to : <image file>
             */
            //post the image to the api


            //wait and return the result

            return result;
        }
    }

    public struct PixelColor
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }
}
