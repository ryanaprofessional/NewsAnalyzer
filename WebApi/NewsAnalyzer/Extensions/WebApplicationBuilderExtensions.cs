using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

namespace Extensions
{
    public static class WebApplicationBuilderExtensions
    {
    public static WebApplicationBuilder UseDynamoDb(this WebApplicationBuilder builder)
        {
            var awsOptions = builder.Configuration.GetAWSOptions();
            var awsAccessKey = builder.Configuration["AwsAccessKey"];
            var awsSecretKey = builder.Configuration["AwsSecretKey"];

            if (awsAccessKey != null && awsSecretKey != null)
            {
                awsOptions.Credentials = new BasicAWSCredentials(builder.Configuration["AwsAccessKey"], builder.Configuration["AwsSecretKey"]);
            }

            builder.Services.AddDefaultAWSOptions(awsOptions);
            builder.Services.AddAWSService<IAmazonDynamoDB>();
            builder.Services.AddTransient<IDynamoDBContext, DynamoDBContext>();
            return builder;
        }
    }
}
