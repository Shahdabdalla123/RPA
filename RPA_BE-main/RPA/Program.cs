
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RPA.Core.Models;
using RPA.Infrastracture.Context;
using RPA.Infrastracture.Services.Abstraction;
using RPA.Infrastracture.Services.Implementation;
using Scalar.AspNetCore;
using System.Text;

namespace RPA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("http://localhost:4200") // Explicitly allow Angular frontend
                          .AllowAnyMethod()                    // Allow GET, POST, OPTIONS, etc.
                          .AllowAnyHeader()                    // Allow Authorization, Content-Type, etc.
                          .AllowCredentials();                 // Support credentialed requests
                });
            });

            //add dbcontext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            //add identity
            builder.Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();
            // Configure Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IAuthService, AuthService>(); 
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            
            var app = builder.Build();
            app.MapOpenApi(); 
            if (app.Environment.IsDevelopment())
            {
                app.MapScalarApiReference();
            }

            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
