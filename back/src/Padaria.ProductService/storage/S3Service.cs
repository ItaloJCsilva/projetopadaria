using Amazon.S3;
using Amazon.S3.Model;

namespace Padaria.ProductService.Storage
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3;
        private readonly IConfiguration _config;

        public S3Service(IConfiguration config)
        {
            _config = config;

            var awsOptions = new AmazonS3Config
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(
                    _config["Aws:Region"])
            };

            _s3 = new AmazonS3Client(
                _config["Aws:AccessKey"],
                _config["Aws:SecretKey"],
                _config["Aws:SessionToken"],
                awsOptions
            );
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var bucket = _config["Aws:BucketName"];
            var fileName = $"{Guid.NewGuid()}-{file.FileName}";

            using var stream = file.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = bucket,
                Key = fileName,
                InputStream = stream,
                ContentType = file.ContentType,
            };
            

            await _s3.PutObjectAsync(request);

            return $"https://{bucket}.s3.amazonaws.com/{fileName}";
        }
    }
}