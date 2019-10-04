FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100 AS build-env
WORKDIR /app

# Copy everything to working dir and restore packages
COPY . .
RUN dotnet restore

# Change working dir to the Blp.NetCoreLearning.WebApp project dir and publish
WORKDIR /app/Blp.NetCoreLearning.WebApp
RUN dotnet publish -c Release -o out

# Change working dir to the Blp.NetCoreLearning.DataMigration project dir and build
WORKDIR /app/Blp.NetCoreLearning.DataMigration
RUN dotnet build -c Release -o out

# build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0
WORKDIR /app

# Copy the published Blp.NetCoreLearning.WebApp
COPY --from=build-env /app/Blp.NetCoreLearning.WebApp/out .

#Copy the built Blp.NetCoreLearning.DataMigration
COPY --from=build-env /app/Blp.NetCoreLearning.DataMigration/out ./Blp.NetCoreLearning.DataMigration

EXPOSE 5000
EXPOSE 80
ENTRYPOINT ["dotnet", "Blp.NetCoreLearning.WebApp.dll"]
