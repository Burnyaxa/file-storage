using System;
using System.Collections.Generic;
using System.Text;
using Amazon.S3;

namespace BLL.Extensions
{
    public static class S3ClientExtensions
    {
        public static string GetResourceUrl(this IAmazonS3 client, string bucket, string key)
        {
            return @$"https://{bucket}.s3.amazonaws.com/{key}";
        }
    }
}
