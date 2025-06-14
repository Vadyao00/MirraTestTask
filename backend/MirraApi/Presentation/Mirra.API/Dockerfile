FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Presentation/Mirra.API/Mirra.API.csproj", "Presentation/Mirra.API/"]
COPY ["Core/Contracts/Contracts.csproj", "Core/Contracts/"]
COPY ["Core/MirraApi.Application/MirraApi.Application.csproj", "Core/MirraApi.Application/"]
COPY ["Core/MirraApi.Domain/MirraApi.Domain.csproj", "Core/MirraApi.Domain/"]
COPY ["Infrastructure/MirraApi.Persistance/MirraApi.Persistance.csproj", "Infrastructure/MirraApi.Persistance/"]
RUN dotnet restore "Presentation/Mirra.API/Mirra.API.csproj"
COPY . .
RUN dotnet build "Presentation/Mirra.API/Mirra.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Presentation/Mirra.API/Mirra.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mirra.API.dll"]