using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CoreCodeCamp.Controllers;
using CoreCodeCamp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CoreCodeCamp
{
  public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CampContext>();
        services.AddScoped<ICampRepository, CampRepository>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddControllers();

            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 1);
                opt.ReportApiVersions = true;
                //opt.ApiVersionReader = new QueryStringApiVersionReader("ver");
                //opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                opt.ApiVersionReader = ApiVersionReader.Combine( 
                    new HeaderApiVersionReader("X-Version"),
                    new QueryStringApiVersionReader("ver", "version")
                    );

                // NOT sure if this still work
                //opt.Conventions.Controller<TalksController>()
                //.HasApiVersion(new ApiVersion(1, 0));
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(cfg =>
      {
        cfg.MapControllers();
      });
    }
  }
}
