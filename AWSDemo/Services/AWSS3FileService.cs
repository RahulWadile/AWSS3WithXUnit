using Amazon.S3.Model;
using AWSDemo.Helpers;
using AWSDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static AWSDemo.Models.EnumModel;

namespace AWSDemo.Services
{

    public interface IAWSS3FileService
    {
        Task<bool> UploadFile(string uploadFileName);
       
        Task <IEnumerable<Catalog>> FilesList();
        Task<Catalog> GetFile(string key);
        Task<bool> UpdateFile(string uploadFileName, string key);
        Task<bool> DeleteFile(string key);
        Task<string> ReadfileContent(string key);


    }
    public class AWSS3FileService : IAWSS3FileService
    {
        private readonly IAWSS3BucketHelper _AWSS3BucketHelper;

        public AWSS3FileService(IAWSS3BucketHelper AWSS3BucketHelper)
        {
            this._AWSS3BucketHelper = AWSS3BucketHelper;
        }
        public async Task<bool> UploadFile(string uploadFileName)
        {
            try
            {
                var path = Path.Combine("Files", uploadFileName.ToString() + ".json");
                using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string fileExtension = Path.GetExtension(path);
                    string fileName = string.Empty;
                    fileName = $"{DateTime.Now.Ticks}{fileExtension}";                    
                    return await _AWSS3BucketHelper.UploadFile(fsSource, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Catalog>> FilesList()
        {
            try
            {
                ListVersionsResponse listVersions = await _AWSS3BucketHelper.FilesList();              
                var files = listVersions.Versions.Select(c => c.Key).ToList();

                List<Catalog> lstCatalog = new List<Catalog>();
                foreach (var fileName in files)
                {
                    Stream stream = await _AWSS3BucketHelper.GetFile(fileName);

                    StreamReader reader = new StreamReader(stream);
                    string text = reader.ReadToEnd();

                    Catalog catalog = System.Text.Json.JsonSerializer.Deserialize<Catalog>(text);
                    lstCatalog.Add(catalog);
                }

                return lstCatalog;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<Catalog> GetFile(string catalogId)
        {
            try
            {
                //Stream fileStream = await _AWSS3BucketHelper.GetFile(key);
                //if (fileStream == null)
                //{
                //    Exception ex = new Exception("File Not Found");
                //    throw ex;
                //}
                //else
                //{
                //    return fileStream;
                //}

                Stream stream = await _AWSS3BucketHelper.GetFile(catalogId.ToString() + ".json");

                StreamReader reader = new StreamReader(stream);
                string text = reader.ReadToEnd();

                Catalog catalog = System.Text.Json.JsonSerializer.Deserialize<Catalog>(text);
                return catalog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> UpdateFile(string uploadFileName, string key)
        {
            try
            {
                var path = Path.Combine("Files", uploadFileName.ToString() + ".json");
                using (FileStream fsSource = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return await _AWSS3BucketHelper.UploadFile(fsSource, key+".json");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> DeleteFile(string key)
        {
            try
            {
                return await _AWSS3BucketHelper.DeleteFile(key);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> ReadfileContent(string key)
        {
            try
            {
                return await _AWSS3BucketHelper.ReadfileContent(key);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }

}
