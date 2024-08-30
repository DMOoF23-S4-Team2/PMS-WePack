#!/bin/bash

# Wait for SQL Server to start
sleep 30s

# Check if the database exists
DB_EXISTS=$(/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Team2Semester4" -Q "IF DB_ID('WePackTest') IS NOT NULL PRINT 'EXISTS'" -h -1)

# Create the database if it doesn't exist
if [ "$DB_EXISTS" != "EXISTS" ]; then
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Team2Semester4" -Q "CREATE DATABASE WePackTest"
fi

# Apply EF Core migrations
dotnet ef database update --project src/PMS.Infrastructure/PMS.Infrastructure.csproj --startup-project src/PMS.API/PMS.API.csproj