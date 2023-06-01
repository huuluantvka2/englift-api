FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EngLift.WebAPI/EngLift.WebAPI.csproj", "EngLift.WebAPI/"]
COPY ["EngLift.UnitTest/EngLift.UnitTest.csproj", "EngLift.UnitTest/"]
COPY ["EngLift.Service/EngLift.Service.csproj", "EngLift.Service/"]
COPY ["EngLift.Sercurity/EngLift.Sercurity.csproj", "EngLift.Sercurity/"]
COPY ["EngLift.Model/EngLift.Model.csproj", "EngLift.Model/"]
COPY ["EngLift.DTO/EngLift.DTO.csproj", "EngLift.DTO/"]
COPY ["EngLift.Data/EngLift.Data.csproj", "EngLift.Data/"]
COPY ["EngLift.Common/EngLift.Common.csproj", "EngLift.Common/"]
RUN dotnet restore "EngLift.WebAPI/EngLift.WebAPI.csproj"

COPY . .
WORKDIR "/src/EngLift.WebAPI"
RUN dotnet build "EngLift.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EngLift.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EngLift.WebAPI.dll"]