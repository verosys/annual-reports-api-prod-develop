using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using AnnualReportsAPI.JWT;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AnnualReportsAPI.Options;
using AnnualReportsAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace AnnualReportsAPI
{
  public class Startup
  {
    private const string SecretKey = "keythi$sk$Fwe47sD2asSAJ32geY4Tthat";
    private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
    private IHostingEnvironment _env { get; }
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration, IHostingEnvironment env)
    {
      Configuration = configuration;
      _env = env;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      if (this._env.IsProduction())
      {
        services.AddMvc(options =>
        {
          options.Filters.Add(new RequireHttpsAttribute());
        });
      }

      services.Configure<ProjectGeneralsOptions>(Configuration.GetSection("ProjectGenerals"));
      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

      //Turn off microsoft claims mapping
      //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

      // Configure JwtIssuerOptions
      // Used in ID by JwtController.cs
      services.Configure<JwtIssuerOptions>(options =>
      {
        options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
        options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
      });

      //Configure SendGrid options
      services.Configure<SendGridOptions>(options =>
      {
        options.ApiKey = Environment.GetEnvironmentVariable("APPSETTING_sendgridkey");
        options.FromAddress = Environment.GetEnvironmentVariable("APPSETTING_sendgridfromaddress");
        options.FromTitle = Environment.GetEnvironmentVariable("APPSETTING_sendgridfromtitle");
      });

      //Configure file storage service
      services.Configure<FileStorageOptions>(options =>
      {
        options.ConnectionString = Environment.GetEnvironmentVariable("AZURE_filestorageconnectionstring");
        options.ShareName = Environment.GetEnvironmentVariable("AZURE_filestorageshare");
      });

      //asp.net core 2.0. JWT
      var tokenValidationParameters = new TokenValidationParameters
      {
        //If we turn of MS claim types mapping then we need to provide custom role claims type here
        //In order for authorization filter to work with roles
        //RoleClaimType = "role",
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = this._signingKey,
        ValidateAudience = false,
        ValidateActor = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.TokenValidationParameters = tokenValidationParameters;
              });

      //Enable CORS
      //START
      services.AddCors(options =>
      {
        options.AddPolicy("CorsPolicy",
        builder => builder.WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .WithExposedHeaders("content-disposition")
        .AllowAnyHeader()
        .AllowCredentials()
        .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));
      });
      //END

      //Add GZIP compression
      //START
      services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
      services.AddResponseCompression(options =>
      {
        options.Providers.Add<GzipCompressionProvider>();
      });
      //END

      //Gfi documents save options
      //START
      services.Configure<GfiUploadOptions>(Configuration.GetSection("GfiUpload"));
      //END

      //DB Connection
      //START
      services.Configure<MongoDBOptions>(options =>
      {
        options.DbName = Environment.GetEnvironmentVariable("APPSETTING_mongodbname");
        options.Url = Environment.GetEnvironmentVariable("APPSETTING_cosmodburl");
      });
      //END

      //Add custom services
      //START
      services.AddSingleton<UsersService>();
      services.AddSingleton<EmailService>();
      services.AddTransient<FileStorageService>();
      //END

      services.AddMvc();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseAuthentication();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRequestLocalization(new RequestLocalizationOptions()
      {
        DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("hr-HR")
      });

      app.UseResponseCompression();
      app.UseCors("CorsPolicy");

      app.UseMvc();
    }
  }
}
