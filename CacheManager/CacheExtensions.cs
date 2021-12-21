using Couchbase.Extensions.Caching;
using Couchbase.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CacheManager
{
    public static class CacheExtensions
    {
        public static IServiceCollection AddDistributedCache(this IServiceCollection services, IConfiguration configuration)
        {
            //activate Couchbase
            services.AddCouchbase(opt =>
            {
                opt.ConnectionString = configuration.GetSection("CouchbaseSettings").GetValue<string>("ConnectionString");
                opt.UserName = configuration.GetSection("CouchbaseSettings").GetValue<string>("Username");
                opt.Password = configuration.GetSection("CouchbaseSettings").GetValue<string>("Password");
            });

            services.AddDistributedCouchbaseCache(configuration.GetSection("CouchbaseSettings").GetValue<string>("BucketName"), opt => { });


            return services;
        }
    }
}
