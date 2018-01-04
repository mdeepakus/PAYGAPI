using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using PAYG.Domain.RepositoryInterfaces;
using PAYG.Domain.Services;
using PAYG.Infrastructure.Authentication;
using PAYG.Infrastructure.Repository;
using PAYG.Server.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using PAYG.Server.Infrastructure.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PAYG.Server.Validators.User;

namespace PAYG.Server
{
    /// <summary>
    /// deepak mittal 
    /// </summary>
    public class Startup
    {
        private const string SecretKey = "mysupersecretkey!123";
        //private const string SecretKey = "paygsecretkey!0108";
        private SymmetricSecurityKey _symmetricKey;

        /// <summary>
        /// Start Up
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
         
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Conventions.Add(new FeatureConvention());
                options.Filters.Add(typeof(DbTransactionFilter));
                options.Filters.Add(typeof(ValidatorActionFilter));
            })

            .AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)

            .AddRazorOptions(razor =>
            {
                // {0} - Action Name
                // {1} - Controller Name
                // {2} - Area Name
                // {3} - Feature Name (Added by Feature Convention)
                // Replace normal view location entirely
                razor.ViewLocationFormats.Clear();
                razor.ViewLocationFormats.Add("/Features/{3}/{1}/{0}.cshtml");
                razor.ViewLocationFormats.Add("/Features/{3}/{0}.cshtml");
                razor.ViewLocationFormats.Add("/Features/Shared/{0}.cshtml");
                razor.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
            })

            .AddFluentValidation(fluent =>
            {
                fluent.RegisterValidatorsFromAssemblyContaining<Startup>();
            })

            .AddViewLocalization()
            .AddDataAnnotationsLocalization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                new Info
                {
                    Title = "Pay As You Go API",
                    Description = "Pay As You Go WEB API",
                    Version = "0.1"
                });
                c.IncludeXmlComments(Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "PAYG.Server.xml"));
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Standard API Bearer Scheme",
                    Name = "Authorization",
                    In = "header"
                });
                c.CustomSchemaIds(x => x.FullName);
            });

            //TODO: Replace with json resource file
            services.AddLocalization(opt => opt.ResourcesPath = "Resources"); // resource file localization

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en-GB")
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en-GB", uiCulture: "en-GB");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            // Mediatr
            services.AddMediatR();

            //Bearer Authentication
            var key = Encoding.UTF8.GetBytes(SecretKey);
            _symmetricKey = new SymmetricSecurityKey(key);

            services.AddOptions();

            //var jwtAppSettingOptions = Configuration.GetSection(nameof(TokenProviderOptions));

            services.Configure<TokenProviderOptions>(options =>
            {
                options.Issuer = Configuration["Logging:TokenProviderOptions:Issuer"];
                options.Audience = Configuration["Logging:TokenProviderOptions:Audience"];
                options.SigningCredentials = new SigningCredentials(_symmetricKey, SecurityAlgorithms.HmacSha256Signature);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme, options =>
               {
                   options.RequireHttpsMetadata = false;
                   options.SaveToken = true;
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidIssuer = Configuration["Logging:TokenProviderOptions:Issuer"],
                       ValidAudience = Configuration["Logging:TokenProviderOptions:Audience"],
                       IssuerSigningKey = _symmetricKey
                   };
               });

            // Database
            //services.TryAddScoped<IDbConnection>(_ => new SqlConnection("Data Source=IN00268;Initial Catalog=SiriusSR15;Persist Security Info=True;User ID=sirius;Password=$1R1U5;"));
            //services.TryAddScoped<IDbConnection>(_ => new SqlConnection(Configuration.GetConnectionString("DefaultConnection")));
            services.TryAddScoped<IDbConnection>(_ => new SqlConnection(Configuration["Logging:ConnectionStrings:DefaultConnection"]));
            services.AddScoped<ITransactionManager, TransactionManager>();
            services.AddScoped(sp =>
            {
                var transactionManager = sp.GetService<ITransactionManager>();
                return transactionManager.CreateTransaction();
            });
            services.AddScoped<IDataRepository, DataRepository>();

            services.TryAddScoped<IVehiclesRepository, VehicleRepository>();
            services.TryAddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPasswordValidator, PasswordValidator>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddScoped<IUserValidator, UserNameValidator>();
            services.AddScoped<IJourneyService, JourneyService>();
            services.AddScoped<IJourneyRepository, JourneyRepository>();
            services.AddScoped<IJourneyDetailsRepository, JourneyDetailsRepository>();

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            app.UseAuthentication();
            // token provider
            //var jwtAppSettingOptions = Configuration.GetSection(nameof(TokenProviderOptions));
            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    ValidateIssuer = true,
            //    ValidIssuer = jwtAppSettingOptions[nameof(TokenProviderOptions.Issuer)],

            //    ValidateAudience = true,
            //    ValidAudience = jwtAppSettingOptions[nameof(TokenProviderOptions.Audience)],

            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = _symmetricKey,

            //    RequireExpirationTime = true,
            //    ValidateLifetime = true,

            //    ClockSkew = TimeSpan.FromMinutes(Convert.ToDouble(jwtAppSettingOptions[nameof(TokenProviderOptions.ClockSkew)]))
            //};

            //// token authentication
            //app.UseAuthentication(new JwtBearerOptions
            //{
            //    AutomaticAuthenticate = true,
            //    AutomaticChallenge = true,
            //    TokenValidationParameters = tokenValidationParameters
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            // standard app configuration
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc();
            

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pay As You Go API");
                c.DocExpansion("none");
                c.ShowRequestHeaders();
            });
            
        }
    }
}
