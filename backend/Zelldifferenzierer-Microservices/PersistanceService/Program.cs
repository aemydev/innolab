namespace PersistanceService
{
    public class Program
    {

        /// <summary>
        /// This Service is a placeholder for all Tasks where you have to storage / retrieve data from a DB / drive
        /// Currently everything is directly called from disk
        /// For a extended implementation:
        ///     Add Consumers for DB / disk CRUD methods
        ///     Add Consumers for complex tasks which are related to persistance
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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


            app.MapControllers();

            app.Run();
        }
    }
}