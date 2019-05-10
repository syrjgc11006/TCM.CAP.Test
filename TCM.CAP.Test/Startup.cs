using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using TCM.CAP.OrderService.Repository;

namespace TCM.CAP.Test
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
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddDbContext<OrderDbContext>();

            //services.AddTransient<ISubscriberService, SubscriberService>();
            // Repository

            services.AddCap(x =>
            {
                x.UseEntityFramework<OrderDbContext>();
                x.UseRabbitMQ(m=> {
                    m.HostName = "peaceService.net";
                    m.VirtualHost = "/prod";
                    m.ExchangeName = "cap.test";
                    m.UserName = "admin";
                    m.Password = "admin";
                    m.Port = 5672;
                });
                x.UseDashboard();
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = (type, name, content) =>
                {
                    Console.WriteLine($@"A message of type {type} failed after executing {x.FailedRetryCount} several times, requiring manual troubleshooting. Message name: {name}, message body: {content}");
                };
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("OrderService", new Info { Title = "TCM.CAP.OrderService", Version = "v1", Contact = new Contact { Email = "285130205@qq.com", Name = "TCM.CAP.OrderService", Url = "http://0.0.0.0" }, Description = "TCM.CAP.OrderService" });
                var basePath = ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "TCM.CAP.OrderService.xml");
                options.IncludeXmlComments(xmlPath);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //var provider = services.BuildServiceProvider();
            //var repository = provider.GetService<IOrderRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseMvc().UseSwagger(options =>
            {
                options.RouteTemplate = "{documentName}/swagger.json";   //documentName与options.SwaggerDoc("API01" 需要保持一致
            })
               .UseSwaggerUI(options =>
               {
                   options.SwaggerEndpoint("/OrderService/swagger.json", "OrderService");
               }); ;
        }
    }
}
