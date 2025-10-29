using Aspire.Hosting.AWS.Lambda;

var builder = DistributedApplication.CreateBuilder(args);

#pragma warning disable CA2252 // This API requires opting into preview features

// Add Lambda emulator
builder.AddAWSLambdaServiceEmulator();

// Add Lambda function (API)
var apiLambda = builder.AddAWSLambdaFunction<Projects.ReColhe_API>("api", "ReColhe.API");

// Add API Gateway emulator
builder.AddAWSAPIGatewayEmulator("api-gateway", APIGatewayType.HttpV2, new APIGatewayEmulatorOptions
{
    Port = 3000
})
.WithReference(apiLambda, Method.Any, "/")
.WithReference(apiLambda, Method.Any, "/{proxy+}");

builder.Build().Run();
