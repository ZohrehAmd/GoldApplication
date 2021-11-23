using GoldApplication.WebAPI.Contexts;
using GoldApplication.WebAPI.Mapper;
using GoldApplication.WebAPI.Services.Account;
using GoldApplication.WebAPI.Services.Event;
using GoldApplication.WebAPI.Services.Product;
using GoldApplication.WebAPI.Services.ProductUser;
using GoldApplication.WebAPI.Services.User;
using GoldApplication.WebAPI.Services.UserEvent;
using GoldApplication.WebAPI.Utilities.Setting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GoldApplication.WebAPI
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

            services.AddCors();

            #region DbContext
            services.AddDbContext<GoldApplicationContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("LocalConnection"))
                );
            #endregion

            #region Mapping
            services.AddAutoMapper(typeof(ModelMapping));
            #endregion

            #region Swagger

            services.AddSwaggerGen(options =>
            {

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Authorization made by JWT.\r\n\r\n"+
                    "For authorize Please enter 'Bearer'+' '+JWT code \r\n\r\n"+  
                    "exam : \"Bearer (Code JWT)\".",

                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                        Scheme="oauth2",
                        Name = "Bearer",
                        In =ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
                options.SwaggerDoc("Gold_Api_Product",
                    new OpenApiInfo()
                    {
                        Title = "Gold Api Product Document",
                        Version = "1",
                        Description = "API Gold Application For Product"
                    });
                options.SwaggerDoc("Gold_Api_ProductUser",
                   new OpenApiInfo()
                   {
                       Title = "Gold Api Product User Document",
                       Version = "1",
                       Description = "API Gold Application For ProductUser",
                       Contact = new OpenApiContact()
                       {
                           Email = "aslani172@gmail.com",
                           Name = "Farhad Aslani",
                           Url = new Uri("https://www.instagram.com/farhad_codeyad/")
                       },
                       License = new OpenApiLicense()
                       {
                           Name = "CodeYad Developers",
                           Url = new Uri("https://www.instagram.com/farhad_codeyad/")
                       }
                   });
                options.SwaggerDoc("Gold_Api_Event",
                   new OpenApiInfo()
                   {
                       Title = "Gold Api Document Event",
                       Version = "1",
                       Description = "API Gold Application For Event"
                   });
                options.SwaggerDoc("Gold_Api_UserEvent",
                 new OpenApiInfo()
                 {
                     Title = "Gold Api Document Event User",
                     Version = "1",
                     Description = "API Gold Application For Event User"
                 });
                options.SwaggerDoc("Gold_Api_Account",
                    new OpenApiInfo()
                    {
                        Title = "Gold Api Account Document",
                        Version = "1",
                        Description = "API Gold Application For Account"
                    });
                options.SwaggerDoc("Gold_Api_User",
                    new OpenApiInfo()
                    {
                        Title = "Gold Api User Document",
                        Version = "1",
                        Description = "API Gold Application For User"
                    });
                var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var cmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                options.IncludeXmlComments(cmlCommentsFullPath);
            });

            #endregion

            #region Authentication GWT

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var Key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x => {
                   x.RequireHttpsMetadata = false;
                   x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
               });

            #endregion

            #region Dependency Injection
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IProductUserRepository, ProductUserRepository>();
            services.AddScoped<IUserEventRepository, UserEventRepository>();
            #endregion

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env  )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/Gold_Api_Product/swagger.json", "Gold Application API Product");
                options.SwaggerEndpoint("/swagger/Gold_Api_ProductUser/swagger.json", "Gold Application API ProductUser");
                options.SwaggerEndpoint("/swagger/Gold_Api_Event/swagger.json", "Gold Application API Event");
                options.SwaggerEndpoint("/swagger/Gold_Api_UserEvent/swagger.json", "Gold Application API Event User");
                options.SwaggerEndpoint("/swagger/Gold_Api_Account/swagger.json", "Gold Application API Account");
                options.SwaggerEndpoint("/swagger/Gold_Api_User/swagger.json", "Gold Application API User");
                options.RoutePrefix = "";
            });

            app.UseRouting();
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
