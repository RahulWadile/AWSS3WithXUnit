using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AWSDemo.Services
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _client;

        public S3Service(IAmazonS3 client)
        {
            _client = client;
        }

        public async Task GetObjectFromS3Async(string bucketName)
        {
            const string keyname = "test.txt";
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyname
                };

                string responseBody;

                using (var response = await _client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content.Type"];

                    Console.WriteLine($"object meta, Title:{ title }");
                    Console.WriteLine($"object meta, Content Type:{ contentType }");
                    responseBody = reader.ReadToEnd();
                }

                var pathAndFileName = $"E:\\S3Temp\\{keyname}";
                var createText = responseBody;
                File.WriteAllText(pathAndFileName, createText);
            }
            catch (AmazonS3Exception ex)
            {
                var pathAndFileName = $"E:\\S3Temp\\{keyname}";
                File.WriteAllText(pathAndFileName, ex.InnerException.Message);
            }
            catch (Exception exa)
            {
                var pathAndFileName = $"E:\\S3Temp\\{keyname}";
                File.WriteAllText(pathAndFileName, exa.Message);
            }
        }

    }
}
