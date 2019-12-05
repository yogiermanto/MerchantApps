using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MerchantApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace MerchantApi
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MerchantDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc()
                .AddJsonOptions(o => o.SerializerSettings.ReferenceLoopHandling =
                                    Newtonsoft.Json.ReferenceLoopHandling.Ignore); ;

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Merchant API", Version = "V1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "post API V1");
            });
        }
    }
}
