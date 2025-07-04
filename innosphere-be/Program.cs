﻿
using dotenv.net;
using innosphere_be.Configurations;
using innosphere_be.Mappings;
using innosphere_be.MiddleWares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entities;

namespace innosphere_be
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            if (File.Exists(".env"))
            {
                DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { ".env" }));
            }


            //set up routing
            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            //set up CORS
            ConfigurationService.SetupCors(builder.Services);

            //set up swagger
            ConfigurationService.SetupSwagger(builder.Services);

            //set up automapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            //set up DB
            ConfigurationService.SetupContextDb(builder.Services, builder.Configuration);

            //set up DI
            ConfigurationService.SetupDI(builder.Services, builder.Configuration);

            //set up JWT authentication
            ConfigurationService.SetupJWT(builder.Services, builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            await using var scope = app.Services.CreateAsyncScope();
            var context = scope.ServiceProvider.GetRequiredService<InnoSphereDBContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "A problem occurred during migration");
            }

            app.Run();
        }
    }
}
