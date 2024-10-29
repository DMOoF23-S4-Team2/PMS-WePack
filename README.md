# PMS-WePack
## Csv and Filenames
filenames should ends with "-command.csv"<br/>
command tells the system how to handle the file<br/>
Available commands:
- create (creates product(s))
- update (updates product(s))
- delete (deletes product(s))

### Docker commands
`docker build`<br/>
`docker compose up`<br/>

### Dotnet commands
`dotnet build`<br/>
`dotnet restore`<br/>
`dotnet list package`<br/>
`dotnet test`<br/>
`dotnet ef database update --project src/PMS.Infrastructure/PMS.Infrastructure.csproj --startup-project src/PMS.API/PMS.API.csproj`<br/>
`dotnet aspnet-codegenerator controller -name CategoryController -async -api -outDir Controllers/Category`<br/>


#### URL
`[localhost:port]/swagger/index.html`