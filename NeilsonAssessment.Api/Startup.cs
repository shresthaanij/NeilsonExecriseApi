using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NeilsonAssessment.Api.Filters;
using NeilsonAssessment.Api.Helpers;
using NeilsonAssessment.Api.Repositories;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace NeilsonAssessment.Api
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
            services.AddMemoryCache();
            services.AddCors();

            //If policy needs to be defined
            //services.AddAuthorization(options => options.AddPolicy("create", p => p.Requirements), "pets:create")

            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                    o.Filters.Add(new ValidateModelAttribute());
                })
                .AddJsonOptions(o =>
                {
                    if(o.SerializerSettings != null)
                    {
                        o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    }
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                
            services.AddScoped<ICarsRepository, CarsRepository>();
            services.AddScoped<IPetsRepository, PetsRepository>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(f =>
            {
                var context = f.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(context);
            });
            services.AddSingleton<MemoryCacheHelper>();

            /*services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new Info { Title = "Neilson Api", Version = "V1", Description = "This api allows to perform CRUD operation on pets and cars." });
                c.IncludeXmlComments(GetXmlCommentsPath());

            });*/

            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(500, exceptionHandlerFeature.Error, exceptionHandlerFeature.Error.Message);
                            logger.LogError(exceptionHandlerFeature.Error.StackTrace);
                        }

                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An unexpected error has occured. Please try again.");
                    });
                });

                app.UseHsts();
            }

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Entities.Car, Models.CarDto>();
                cfg.CreateMap<Models.CarDto, Entities.Car>();
                cfg.CreateMap<Entities.Pet, Models.PetDto>();
                cfg.CreateMap<Models.PetDto, Entities.Pet>();
            });

            app.UseCors(cfg =>
            {
                cfg.AllowAnyHeader();
                cfg.AllowAnyMethod();
                cfg.AllowAnyOrigin();
            });

            app.UseMiddleware<Helpers.AuthenticationMiddleware>();

            app.UseResponseCaching();

            app.UseHttpsRedirection();
            app.UseMvc();

            /*app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                var dir = AppDomain.CurrentDomain.BaseDirectory;
                c.SwaggerEndpoint(System.IO.Path.Combine("/swagger/v1/swagger.json"), "Neilson API");
            });*/
        }
    }
}
