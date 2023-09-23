#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SpendManagement.Domain.API/SpendManagement.Domain.API.csproj", "SpendManagement.Domain.API/"]
COPY ["src/Crosscutting/Crosscutting.csproj", "Crosscutting/"]
COPY ["src/Application.Kafka/Application.Kafka.csproj", "Application.Kafka/"]
COPY ["src/Data/Data.csproj", "Data/"]
COPY ["src/Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "SpendManagement.Domain.API/SpendManagement.Domain.API.csproj"
COPY . .

RUN dotnet build "src/SpendManagement.Domain.API/SpendManagement.Domain.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/SpendManagement.Domain.API/SpendManagement.Domain.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpendManagement.Domain.API.dll"]