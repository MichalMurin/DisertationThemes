
namespace DissertationThemes.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var factory = new EndpointsFactory(app);
            factory.CreateGetStProgramsEndpoint();
            factory.CreateGetThemeEndpoint();
            factory.CreateGetDocxThemeEndpoint();
            factory.CreateGetThemesByYearAndProgramEndpoint();
            factory.CreateGetThemes2CsvEndpoint();
            factory.CreateGetThemesYearsEndpoint();

            app.MapControllers();

            app.Run();
        }
    }
}
