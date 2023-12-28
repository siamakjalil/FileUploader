using FileUploader.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;  
using FileUploader.Models;
using FileUploader.Repositories;

namespace FileUploader.Config
{
    public static class FileUploaderServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BucketConfig>(config =>
                {
                    config.AccessKey = configuration["BucketConfig:AccessKey"];
                    config.BucketName = configuration["BucketConfig:BucketName"];
                    config.EndPoint = configuration["BucketConfig:EndPoint"];
                    config.SecretKey = configuration["BucketConfig:SecretKey"];
                }
            );
            services.AddScoped<IAws3Services, Aws3Services>();
            services.AddSingleton<AwsUpload>();
            
            return services;
        }
    }
}
