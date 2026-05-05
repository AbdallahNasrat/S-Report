using Application.DI;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Infrastructure;
using Infrastructure.SecurityService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using Microsoft.OpenApi.Models;
using SReport.Api.Middlewares;
using SReport.Hubs;
using System.Text;
using System.Text.Json.Serialization;
namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.

            builder.Services.AddSignalR();


            builder.Services.AddControllers()
                .AddJsonOptions(options => {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "S-Report API", Version = "v1" });

                //  (JWT Bearer)
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                opt.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[]{}
                    }
                 });
            });

            //DB -- Link Database At infrastructure project with Presentation Project
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationService();
            var jwtSections = builder.Configuration.GetSection("JwtSettings");
            var jwtOptions = jwtSections.Get<JwtOptions>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = false,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            FirebaseApp.Create(new AppOptions() {
                Credential = GoogleCredential.FromFile("firebase-config.json")
            });


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed(origin => true) // دي التريكة اللي بتسمح لأي فرونت إند يكلمك
                          .AllowCredentials(); // دي عشان الـ SignalR والـ Cookies لو بتستخدموها
                });
            });


            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
                app.UseSwaggerUI();
          //  }
            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapHub<ReportHub>("/reportHub");

            app.MapControllers();

            app.Run();
        }
    }
}
