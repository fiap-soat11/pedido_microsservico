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
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;

namespace WebAPI.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfraStructure(this IServiceCollection Services, IConfiguration configuration)
        {

            #region conexões AWS DynamoDB
            
            var serviceUrl = configuration["AWS:DynamoDB:ServiceURL"];
            var region = configuration["AWS:Region"] ?? "us-east-1";
            
            if (!string.IsNullOrEmpty(serviceUrl))
            {
                var credentials = new BasicAWSCredentials("fakeAccessKey", "fakeSecretKey");
                var config = new AmazonDynamoDBConfig
                {
                    ServiceURL = serviceUrl,
                    //RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region),
                    

                };
                Services.AddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(credentials, config));
            }
            else
            {
                var awsOptions = configuration.GetAWSOptions();
                Services.AddDefaultAWSOptions(awsOptions);
                Services.AddAWSService<IAmazonDynamoDB>();
            }
            
            Services.AddScoped<DynamoDbContext>();
            Services.AddScoped<IDataSource, DataSource.DataSource>();

            #endregion


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