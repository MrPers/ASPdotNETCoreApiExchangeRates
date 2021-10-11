using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.Services;
using WebApplication.Mappings;
using WebApplication.Repository;
using WebApplication.DB;
using Microsoft.AspNetCore.Http.Features;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FormOptions>(options => {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });
            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(ICurrencyRepository), typeof(CurrencyRepository));

            services.AddAutoMapper(typeof(UserMapper));

            services.AddControllers();
            services.AddCors();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(connection));

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                });
            services.AddControllersWithViews();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseWhen(context => 
            //            context.Request.Path.StartsWithSegments("/api"), 
            //            appBuilder =>
            //            {
            //                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = null;
            //                //TODO: take next steps
            //            });
            app.UseRouting();

            // подключаем CORS
            app.UseCors(builder => builder
            //.WithOrigins("http://localhost:4200/")
            .AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseEndpoints(x => x.MapControllers());
           
        }
    }
}
