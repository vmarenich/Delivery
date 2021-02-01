using DeliveryWebApi.DAL;
using DeliveryWebApi.Domain;
using DeliveryWebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unity;

namespace DeliveryWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddControllersAsServices()
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen();
        }

        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(IUnityContainer container)
        {
            // Could be used to register more types
            container.RegisterSingleton<IOffersAppService, OffersAppService>();
            RegisterRepository<Supplier>(container);
            RegisterRepository<Offer>(container);
        }

        private void RegisterRepository<TEntity>(IUnityContainer container)
            where TEntity : EntityBase
        {
            container.RegisterSingleton<IRepository<TEntity>, Repository<TEntity>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Delivery API v1");
                });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
