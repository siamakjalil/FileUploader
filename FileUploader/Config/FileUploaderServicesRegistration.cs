using FileUploader.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;  
using FileUploader.Models;
using FileUploader.Repositories;

namespace FileUploader.Config
{
    public static class FileUploaderServicesRegistration
    {
        public static IServiceCollection ConfigureFileUploaderServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BucketConfig>(config =>
                {
                    config.AccessKey = configuration["BucketConfigs:AccessKey"];
                    config.BucketName = configuration["BucketConfigs:BucketName"];
                    config.EndPoint = configuration["BucketConfigs:EndPoint"];
                    config.SecretKey = configuration["BucketConfigs:SecretKey"];
                }
            );
            services.AddScoped<IAws3Services, Aws3Services>();
            services.AddSingleton<AwsUpload>();
            
            return services;
        }
    }
}
