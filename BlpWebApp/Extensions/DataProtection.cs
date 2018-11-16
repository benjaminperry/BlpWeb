using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BlpWebApp.Extensions
{
    public static class DataProtection
    {
        public static IDataProtectionBuilder SetupDataProtection(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetSection("DataProtection")["ConnectionString"];
            string containerName = configuration.GetSection("DataProtection")["ContainerName"];
            string keyFileName = configuration.GetSection("DataProtection")["KeyFileName"];

            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference(containerName);

            container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            return services
                .AddDataProtection()
                .PersistKeysToAzureBlobStorage(container, keyFileName);
        }
    }
}
