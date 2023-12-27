using System;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileUploader.Config
{
    public static class FileUploaderServicesRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<BucketConfig>(configuration.GetSection("BucketConfig"));
            
            return services;
        }
    }
}
