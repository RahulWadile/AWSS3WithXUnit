using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AWSDemo.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;

namespace AWSDemo.Helpers
{
    public interface IAWSS3BucketHelper
    {
        Task<bool> UploadFile(System.IO.Stream inputStream, string fileName);
        Task<ListVersionsResponse> FilesList();
        Task<Stream> GetFile(string key);
        

        Task<bool> DeleteFile(string key);

        Task<string> ReadfileContent(string key);

    }
    public class AWSS3BucketHelper : IAWSS3BucketHelper
    {
        private readonly IAmazonS3 _amazonS3;
        private readonly ServiceConfiguration _settings;

        public AWSS3BucketHelper(IAmazonS3 s3Client, IOptions<ServiceConfiguration> settings)
        {
            // this._amazonS3 = s3Client;
            //  this._settings = settings.Value;
            //  this._amazonS3 = new AmazonS3Client(this._settings.AWSS3.AccessKey, this._settings.AWSS3.SecretKey, RegionEndpoint.USEast2);           

            this._amazonS3 = new AmazonS3Client("AKIAYATELLXWTGPB7APF", "okpKDK9UxkojC2Ny3t+L9LFDijqbe6dRdUdybkzz", RegionEndpoint.USEast2);
        }
        public async Task<bool> UploadFile(System.IO.Stream inputStream, string fileName)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest()
                {
                    InputStream = inputStream,
                    BucketName = "catlog-bucket",
                    Key = fileName
                };
                PutObjectResponse response = await _amazonS3.PutObjectAsync(request);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task <ListVersionsResponse> FilesList()
        {
            //
            // return await _amazonS3.ListVersionsAsync(_settings.AWSS3.BucketName);
            return await _amazonS3.ListVersionsAsync("catlog-bucket");
        }
        public async Task<Stream> GetFile(string key)
        {

            //  GetObjectResponse response = await _amazonS3.GetObjectAsync(_settings.AWSS3.BucketName, key);
            GetObjectResponse response = await _amazonS3.GetObjectAsync("catlog-bucket", key);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                return response.ResponseStream;
            else
                return null;


        }

        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                // DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync(_settings.AWSS3.BucketName, key);
                DeleteObjectResponse response = await _amazonS3.DeleteObjectAsync("catlog-bucket", key);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<string> ReadfileContent(string key)
        {
            key = key + ".json";
            GetObjectRequest request = new GetObjectRequest
            {
                // BucketName = _settings.AWSS3.BucketName,
                BucketName = "catlog-bucket",
                Key = key,
            };

            using (GetObjectResponse response = await _amazonS3.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                // Assume you have "title" as medata added to the object.
                string title = response.Metadata["x-amz-meta-title"];
                string contentType = response.Headers["Content-Type"];

                string titlaste = $"Object metadata, Title: {title}";
                string contetyptestr = $"Content type: {contentType}";

                // Retrieve the contents of the file.
                string responseBody1 = reader.ReadToEnd();
                //dynamic metaData = JsonConvert.DeserializeObject(responseBody1);
                // Write the contents of the file to disk.

                return responseBody1;
            }

            //string responseBody = "";
            //try
            //{
            //    GetObjectRequest request = new GetObjectRequest
            //    {
            //        BucketName = bucketname,//"test-samples-bucket",
            //        Key = key
            //    };
            //    using (GetObjectResponse response = await _amazonS3.GetObjectAsync(request))
            //    using (Stream responseStream = response.ResponseStream)
            //    using (StreamReader reader = new StreamReader(responseStream))
            //    {
            //        string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
            //        string contentType = response.Headers["Content-Type"];
            //        //Console.WriteLine("Object metadata, Title: {0}", title);
            //        //Console.WriteLine("Content type: {0}", contentType);

            //        responseBody = reader.ReadToEnd(); // Now you process the response body.
            //    }
            //}
            //catch (AmazonS3Exception e)
            //{
            //   // Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            //}
            //catch (Exception e)
            //{
            //   // Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            //}



            //{
            //    var response = await _amazonS3.SelectObjectContentAsync(new SelectObjectContentRequest()
            //    {
            //        Bucket = "test-samples-bucket",
            //        Key = "Sample.json",
            //        ExpressionType = ExpressionType.SQL,
            //       Expression = "select * from S3Object s",
            //        InputSerialization = new InputSerialization()
            //        {
            //            JSON = new JSONInput()
            //            {
            //                JsonType = JsonType.Lines
            //            }
            //        },
            //        OutputSerialization = new OutputSerialization()
            //        {
            //            JSON = new JSONOutput()
            //        }
            //    });






            //    return response.Payload;


            //}
        }


       
    }
}
