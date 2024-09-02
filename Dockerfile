# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/PMS.API/PMS.API.csproj", "src/PMS.API/"]
COPY ["src/PMS.Infrastructure/PMS.Infrastructure.csproj", "src/PMS.Infrastructure/"]
COPY ["src/PMS.Application/PMS.Application.csproj", "src/PMS.Application/"]
COPY ["src/PMS.Core/PMS.Core.csproj", "src/PMS.Core/"]
RUN dotnet restore "src/PMS.API/PMS.API.csproj"
COPY . .
WORKDIR "/src/src/PMS.API"
RUN dotnet build "PMS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PMS.API.csproj" -c Release -o /app/publish

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PMS.API.dll"]