using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using BLL.DTO;
using BLL.Extensions;
using BLL.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BLL.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly RegionEndpoint _bucketRegion = RegionEndpoint.EUWest2;
        private readonly IAmazonS3 _s3Client;

        public CloudStorageService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<string> UploadFileAsync(FileDto file, string bucketName)
        {
            string key = file.CreatorId + "/" + DateTime.Now + "--" + file.Data.Name;
            var fileTransferUtility = new TransferUtility(_s3Client);
            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = file.Data.OpenReadStream(),
                Key = key,
                BucketName = bucketName,
                AutoCloseStream = true,
                CannedACL = S3CannedACL.PublicRead
            };

            await fileTransferUtility.UploadAsync(uploadRequest);

            return _s3Client.GetResourceUrl(bucketName, key);
        }

        public async Task DeleteFileAsync(string bucketName, string link)
        {
            string key = _s3Client.GetKey(bucketName, link);
            await _s3Client.DeleteObjectAsync(bucketName, key);
        }

        public string DownloadFile(string bucketName, string link)
        {
            string key = _s3Client.GetKey(bucketName, link);

            var request = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.Now.AddDays(1)
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }
}
