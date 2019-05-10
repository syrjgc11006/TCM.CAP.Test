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
using TCM.CAP.DeliveryService.Services;

namespace TCM.CAP.DeliveryService
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
            services.AddDbContext<DeliveryDbContext>();

            // Subscriber
            services.AddTransient<IOrderSubscriberService, OrderSubscriberService>();

            services.AddCap(x =>
            {
                x.UseEntityFramework<DeliveryDbContext>();
                x.UseRabbitMQ(m =>
                {
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
                options.SwaggerDoc("DeliveryService", new Info { Title = "TCM.CAP.DeliveryService", Version = "v1", Contact = new Contact { Email = "285130205@qq.com", Name = "TCM.CAP.DeliveryService", Url = "http://0.0.0.0" }, Description = "TCM.CAP.DeliveryService" });
                var basePath = ApplicationEnvironment.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "TCM.CAP.DeliveryService.xml");
                options.IncludeXmlComments(xmlPath);
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                 options.SwaggerEndpoint("/DeliveryService/swagger.json", "DeliveryService");
             }); ;
        }
    }
}
