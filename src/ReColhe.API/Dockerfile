# Dockerfile para AWS Lambda (.NET 9)
# Este Dockerfile é para build local. Para deployment real, use: dotnet lambda deploy-function

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copia csproj files e restaura dependências
COPY ["src/ReColhe.API/ReColhe.API.csproj", "src/ReColhe.API/"]
COPY ["src/ReColhe.Application/ReColhe.Application.csproj", "src/ReColhe.Application/"]
COPY ["src/ReColhe.Domain/ReColhe.Domain.csproj", "src/ReColhe.Domain/"]
COPY ["ReColhe.ServiceDefaults/ReColhe.ServiceDefaults.csproj", "ReColhe.ServiceDefaults/"]

RUN dotnet restore "src/ReColhe.API/ReColhe.API.csproj"

# Copia tudo e publish
COPY . .
WORKDIR "/src/src/ReColhe.API"
RUN dotnet publish "ReColhe.API.csproj" \
    -c Release \
    -o /app/publish

# Para produção, use Container Image com Lambda
FROM public.ecr.aws/lambda/dotnet:9
WORKDIR /var/task
COPY --from=build /app/publish .
# For .NET 9 with AddAWSLambdaHosting(LambdaEventSource.HttpApi),
# use the assembly entry point. The Program.cs Main method is the entry point.
# Handler format: AssemblyName::Namespace.Class::Method
CMD ["ReColhe.API::LambdaEntryPoint::FunctionHandlerAsync"]

