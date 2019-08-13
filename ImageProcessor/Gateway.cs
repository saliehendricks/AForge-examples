using RestSharp;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ImageProcessor
{
    public interface IGatway
    {
        Task<string> MakePredictionRequest(string url, string resource, string filepath);
    }

    public interface IImageProcessor
    {

    }

    public static class ImageProcessor
    {
        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }

    public class Gateway : IGatway
    {
        //Iteration 4 Sample Http call
        //https://eastus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/204f0dbd-4f4c-47ed-9382-120de54c81b5/classify/iterations/Iteration4/image
        //Set Prediction-Key Header to : a8435b876e914433aba85a3f8793a28a
        //Set Content-Type Header to : application/octet-stream
        //Set Body to : <image file>
        public async Task<string> MakePredictionRequest(string url = "https://eastus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/204f0dbd-4f4c-47ed-9382-120de54c81b5/classify/iterations/Iteration4/image", string resource = "", string fullfilepath="")
        {
            string result;
            try
            {
                var client = new HttpClient();

                // Request headers - replace this example key with your valid Prediction-Key.
                client.DefaultRequestHeaders.Add("Prediction-Key", "a8435b876e914433aba85a3f8793a28a");
                               

                HttpResponseMessage response;

                // Request body. Try this sample with a locally stored image.
                byte[] byteData = ImageProcessor.GetImageAsByteArray(fullfilepath);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response = await client.PostAsync(url, content);
                    result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
    }
}
