using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Phonebook.API.Data;
using Phonebook.API.Helpers;
using Phonebook.API.Middleware;
using Phonebook.API.Utils;

namespace Phonebook.API
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
            services.AddDbContext<DataContext>(x => 
                x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
            services.AddCors();
            services.AddAutoMapper(typeof(Startup));
            
            ConfigureUtils(services);
            ConfigureRepositories(services);
            ConfigureJwtBearerAuthentication(services);

        }

        private void ConfigureUtils(IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator,JwtTokenGenerator>();
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IAuthRepository, AuthRepository>(); 
            services.AddScoped<IPhonebookRepository,PhonebookRepository>();
        }
        private void ConfigureJwtBearerAuthentication(IServiceCollection services)
        {
          services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => {
              opt.TokenValidationParameters = new TokenValidationParameters
              {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                  ValidateIssuer = false,
                  ValidateAudience = false
              };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.ConfigureCustomExceptionMiddleware();

            app.UseRouting();

            app.UseCors(cors => cors.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader() );
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
