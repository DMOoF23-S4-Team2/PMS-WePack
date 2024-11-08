# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8080

# Use the SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy and restore dependencies for the API project
COPY ["src/PMS.API/PMS.API.csproj", "src/PMS.API/"]
COPY ["src/PMS.Infrastructure/PMS.Infrastructure.csproj", "src/PMS.Infrastructure/"]
COPY ["src/PMS.Application/PMS.Application.csproj", "src/PMS.Application/"]
COPY ["src/PMS.Core/PMS.Core.csproj", "src/PMS.Core/"]
RUN dotnet restore "src/PMS.API/PMS.API.csproj"

# Copy and restore dependencies for the Web project
COPY ["src/PMS.Web/PMS.Web.csproj", "src/PMS.Web/"]
RUN dotnet restore "src/PMS.Web/PMS.Web.csproj"

# Copy the entire source code and build the API project
COPY . .
WORKDIR "/src/src/PMS.API"
RUN dotnet build "PMS.API.csproj" -c Release -o /app/build

# Copy the entire source code and build the Web project
WORKDIR "/src/src/PMS.Web"
RUN dotnet build "PMS.Web.csproj" -c Release -o /app/build

FROM build AS publish
# Publish the API project
WORKDIR "/src/src/PMS.API"
RUN dotnet publish "PMS.API.csproj" -c Release -o /app/publish/api

# Publish the Web project
WORKDIR "/src/src/PMS.Web"
RUN dotnet publish "PMS.Web.csproj" -c Release -o /app/publish/web

# Use the runtime image to run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/api .
COPY --from=publish /app/publish/web .

# Run the API project
ENTRYPOINT ["dotnet", "PMS.API.dll"]