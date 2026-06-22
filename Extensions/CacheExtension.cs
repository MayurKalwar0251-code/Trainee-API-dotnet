public static class CacheExtension
{
    public static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache( options =>
        {
            options.Configuration = configuration.GetConnectionString("RedisConnection");
            options.InstanceName = configuration.GetConnectionString("RedisInstanceName");
        }); 
        return services;
    } 
}