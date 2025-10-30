using Adapters.Controllers;
using Adapters.Controllers.Interfaces;
using Adapters.Gateways;
using Adapters.Gateways.Interfaces;

using Application.Interfaces;
using Application.UseCases;
using DataSource.Context;
using DataSource.Repositories;
using DataSource.Repositories.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace WebAPI.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection Services, IConfiguration configuration)
        {

            #region conexões
            var mySqlConnectionString = configuration.GetConnectionString("DefaultConnection");
            /* serviços de banco de dados MySql  */
            
            Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySql(mySqlConnectionString,ServerVersion.AutoDetect(mySqlConnectionString))
               .UseLoggerFactory(
                   LoggerFactory.Create(
                       b => b
                           .AddConsole()
                           .AddFilter(level => level >= LogLevel.Information)))
               .EnableSensitiveDataLogging()
               .EnableDetailedErrors();
            });

            #endregion


           

            /* ***** serviços de acesso a base ***** */
            Services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));
            Services.AddScoped<IDataSource, DataSource.DataSource>();


            /* ***** serviços de orquestração ***** */
            
            Services.AddScoped<IPedidoController, PedidoController>();
            
            /* ***** serviços de acesso a dados ***** */
            Services.AddScoped<IPedidoGateway, PedidoGateway>();
            
            /* ***** serviços de negocio ***** */
            Services.AddScoped<IPedidoUseCase, PedidoUseCase>();

            Services.AddScoped<IPedidoRepository, PedidoRepository>();
            
            return Services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddFluentValidationAutoValidation();
            //Services.AddValidatorsFromAssemblyContaining<ClienteRequestValidator>();
            
            return Services;
        }



    }


}