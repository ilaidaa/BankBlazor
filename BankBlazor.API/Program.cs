using Microsoft.EntityFrameworkCore;
using BankBlazor.API.Models; 
namespace BankBlazor.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Denna rad �r till f�r customerController
            builder.Services.AddDbContext<BankContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

           

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles; //Denna rad la jag till f�r att min AccountController skulle funka annars gav den error i Swagger annars blev det evig cirkel n�r jag f�rs�kte f� transaktioner fr�n ett konto
                                                                                                                                   // "Om du st�ter p� ett objekt som pekar tillbaka (ex: Account har Transaction som har Account igen)... d� ska du bara sluta f�lja det d�r � s� att det inte blir en evig loop."
                });



            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();




            // Beh�ver kanske tabort testar
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });








            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // TESTAR kanska ska tabort raden under
            app.UseCors();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
