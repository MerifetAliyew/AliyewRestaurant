using AliyewRestaurant.Application.Abstracts.Services;
using AliyewRestaurant.Persistence.Services;
using AliyewRestaurant.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using AliyewRestaurant.Application.Abstracts.Repositories;
using AliyewRestaurant.Persistence.Repositories;

namespace AliyewRestaurant.Persistence;

public static class ServiceRegistration
{
    public static void RegisterService(this IServiceCollection services)
    {
        #region Repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        #endregion

        #region Servicies
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IMenuItemService, MenuItemService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRoleService, RoleService>();
        #endregion


    }
}