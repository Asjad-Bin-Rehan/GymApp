#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7087
EXPOSE 7088
EXPOSE 7089

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Host/CSAPI/CSAPI.csproj", "Host/CSAPI/"]
COPY ["Infrastructure/CSPInfra/BS/BS.csproj", "Infrastructure/CSPInfra/BS/"]
COPY ["Utility/EndpointExposerGRPC/EndpointGRPC.csproj", "Utility/EndpointExposerGRPC/"]
COPY ["Utility/Helpers/Helpers.csproj", "Utility/Helpers/"]
COPY ["Utility/NATSNotificationSystem/NATSNotificationSystem.csproj", "Utility/NATSNotificationSystem/"]
COPY ["Utility/SessionManager/SessionManager.csproj", "Utility/SessionManager/"]
COPY ["Utility/UserActivity/UserActivity.csproj", "Utility/UserActivity/"]
COPY ["Infrastructure/CSPInfra/DA/DA.csproj", "Infrastructure/CSPInfra/DA/"]
COPY ["Utility/GenericRepository/GenericRepository.csproj", "Utility/GenericRepository/"]
COPY ["Infrastructure/CSPInfra/DM/DM.csproj", "Infrastructure/CSPInfra/DM/"]
COPY ["Utility/AuthProvider/AuthProvider.csproj", "Utility/AuthProvider/"]
COPY ["Utility/CustomHTTP/CustomHTTP.csproj", "Utility/CustomHTTP/"]
COPY ["Utility/Logger/Logger.csproj", "Utility/Logger/"]
RUN dotnet restore "./Host/CSAPI/./CSAPI.csproj"
COPY . .
WORKDIR "/src/Host/CSAPI"
RUN dotnet build "./CSAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CSAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSAPI.dll"]