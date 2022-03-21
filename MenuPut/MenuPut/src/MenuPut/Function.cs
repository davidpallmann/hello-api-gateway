using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace MenuPut
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
        /// Store menu JSON in S3 menu object. 
        /// </summary>
        /// <param name="request">API Gateway proxy request containing body (menu JSON) and optional path parameter 'name'.</param>
        /// <param name="context">Lambda context object used for logging</param>
        /// <returns></returns>
        public async Task<APIGatewayProxyResponse> FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
        {
            try
            {
                var menuJson = request.Body;
                context.Logger.LogLine($"Menu JSON: {menuJson}");

                var name = "menu";
                if (request.PathParameters != null && request.PathParameters.ContainsKey("name"))
                {
                    name = request.PathParameters["name"];
                }

                context.Logger.LogLine($"Updating menu {name}");
                await PutMenu(name, menuJson);

                return new APIGatewayProxyResponse
                {
                    StatusCode = 200,
                    Body = $"Updated menu {name}.json"
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

        private async Task PutMenu(string name, string menuJson)
        {
            var response = await S3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest()
            {
                BucketName = BUCKET,
                Key = name + ".json",
                ContentType = "text/json",
                ContentBody = menuJson
            });
        }
    }
}