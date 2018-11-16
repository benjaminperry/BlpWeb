FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy everything to working dir and restore packages
COPY . .
RUN dotnet restore

# Change working dir to the BlpWebApp project dir and publish
WORKDIR /app/BlpWebApp
RUN dotnet publish -c Release -o out

# Change working dir to the DataMigration project dir and build
WORKDIR /app/DataMigration
RUN dotnet build -c Release -o out

# build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app

# Copy the published BlpWebApp
COPY --from=build-env /app/BlpWebApp/out .

#Copy the built DataMigration
COPY --from=build-env /app/DataMigration/out ./DataMigration

EXPOSE 5000
EXPOSE 80
ENTRYPOINT ["dotnet", "BlpWebApp.dll"]
