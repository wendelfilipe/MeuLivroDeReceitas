using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Criptography;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application
{
    public static class DependencyInjectionExtension
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration )
        {
            AddAutoMapper(services);
            AddUseCase(services);
            AddPasswordEncripter(services, configuration);
        }

        public static void AddAutoMapper(IServiceCollection services)
        {
            // Mapear a request em uma entidade
            services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddUseCase(IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        }

        private static void AddPasswordEncripter(IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetValue<string>("Setting:Password:AdditionalKey");
            services.AddScoped(options => new PasswordEncripter(connection!));
        }
    }
}
