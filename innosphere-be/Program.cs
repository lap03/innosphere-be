
using innosphere_be.Configurations;
using innosphere_be.Mappings;

namespace innosphere_be
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //add register DB
            ConfigurationService.RegisterContextDb(builder.Services, builder.Configuration);

            //add register DI
            ConfigurationService.RegisterDI(builder.Services, builder.Configuration);

            //add automapper
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            //set up swagger
            ConfigurationService.SetupSwagger(builder.Services);

            //
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
