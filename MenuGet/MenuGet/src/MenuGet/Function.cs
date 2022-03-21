using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MenuGet
{
    public class Function
    {
        const string BUCKET = "[BUCKET-NAME]";

        IAmazonS3 S3Client { get; set; }

        /// <summary>
        /// Constructor. Create S3 client.
        /// </summary>
        public Function()
        {
            S3Client = new AmazonS3Client();
        }

        /// <summary>
        /// Constructs an instance with a preconfigured S3 client. This can be used for testing the outside of the Lambda environment.
        /// </summary>
        /// <param name="s3Client"></param>
        public Function(IAmazonS3 s3Client)
        {
            this.S3Client = s3Client;
        }

        /// <summary>
        /// Retrieve menu JSON from S3 menu object. 
        /// </summary>
        /// <param name="request">API Gateway proxy request.</param>
        /// <param name="context">Lambda context object used for logging</param>
        /// <returns>menu JSON</returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                context.Logger.LogLine($"Retrieving menu");

                var name = "menu";
                if (request.PathParameters != null && request.PathParameters.ContainsKey("name"))
                {
                    name = request.PathParameters["name"];
                }

                var menuJson = await GetMenu(name);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = menuJson
                };
            }
            catch (Exception ex)
            {
                return new APIGatewayProxyResponse
                {
                    StatusCode = 500,
                    Body = ex.ToString()
                };
            }
        }

        private async Task<string> GetMenu(string name)
        {
            var response = await S3Client.GetObjectAsync(BUCKET, name + ".json");
            using (var reader = new StreamReader(response.ResponseStream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}