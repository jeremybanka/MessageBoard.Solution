using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MessageBoard.Models;
using MessageBoard.Helpers;
using MessageBoard.Services;

namespace MessageBoard
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors(options =>
        {
          options.AddDefaultPolicy(
            builder =>
            {
              builder.WithOrigins("http://example.com",
                                  "http://www.contoso.com");
            });
        }
      );

      services.AddApiVersioning(o =>
        {
          o.ReportApiVersions = true;
          o.AssumeDefaultVersionWhenUnspecified = true;
          o.DefaultApiVersion = new(0, 0);
        }
      );

      services.AddDbContext<MessageBoardContext>(opt =>
             opt.UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));
      services.AddControllers();

      // configure strongly typed settings object
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

      // configure DI for application services
      services.AddScoped<IUserService, UserService>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // app.UseHttpsRedirection();

      app.UseRouting();

      app.UseCors();

      app.UseAuthorization();

      // custom jwt auth middleware
      app.UseMiddleware<JwtMiddleware>();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
