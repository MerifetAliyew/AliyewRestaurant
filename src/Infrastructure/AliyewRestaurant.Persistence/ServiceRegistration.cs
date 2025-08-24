using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Persistence.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AliyewRestaurant.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        #endregion

        #region Servicies
        services.AddScoped<IUserService, UserService>();
        #endregion


    }
}