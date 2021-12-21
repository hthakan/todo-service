#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["HXFMerchantMicroservices/Services/Order.Service/Order.API/Order.API.csproj", "HXFMerchantMicroservices/Services/Order.Service/Order.API/"]
COPY ["HXFMerchantMicroservices/Services/Order.Service/Order.DomainServices/Order.DomainServices.csproj", "HXFMerchantMicroservices/Services/Order.Service/Order.DomainServices/"]
COPY ["HXFMerchantMicroservices/BuildingBlocks/MongoDBManager/MongoDBManager.csproj", "HXFMerchantMicroservices/BuildingBlocks/MongoDBManager/"]
COPY ["HXFMerchantMicroservices/Services/Order.Service/Order.Domain/Order.Domain.csproj", "HXFMerchantMicroservices/Services/Order.Service/Order.Domain/"]
COPY ["HXFMerchantMicroservices/BuildingBlocks/ElasticsearchManager/ElasticsearchManager.csproj", "HXFMerchantMicroservices/BuildingBlocks/ElasticsearchManager/"]
COPY ["HXFMerchantMicroservices/BuildingBlocks/EventBus/EventBus.csproj", "HXFMerchantMicroservices/BuildingBlocks/EventBus/"]
COPY ["HXFMerchantMicroservices/Services/Order.Service/Order.Infrastructure/Order.Infrastructure.csproj", "HXFMerchantMicroservices/Services/Order.Service/Order.Infrastructure/"]
COPY ["HXFMerchantMicroservices/BuildingBlocks/DapperGenericRepository/DapperGenericRepository.csproj", "HXFMerchantMicroservices/BuildingBlocks/DapperGenericRepository/"]
COPY ["HXFMerchantMicroservices/BuildingBlocks/Common/Common.csproj", "HXFMerchantMicroservices/BuildingBlocks/Common/"]
COPY ["HXFMerchantMicroservices/BuildingBlocks/ErrorHandler/ErrorHandler.csproj", "HXFMerchantMicroservices/BuildingBlocks/ErrorHandler/"]
RUN dotnet restore "HXFMerchantMicroservices/Services/Order.Service/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/HXFMerchantMicroservices/Services/Order.Service/Order.API"
RUN dotnet build "Order.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.API.dll"]