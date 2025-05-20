
using Microsoft.EntityFrameworkCore;
using PollChatApi.Model;
using PollChatApi.Service;
using PollChatApi.Service.Background;
using System;


namespace PollChatApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<PollServices>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddRazorPages();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    policy => policy
                        .AllowAnyOrigin()      // Allow requests from any domain
                        .AllowAnyMethod()      // Allow GET, POST, etc.
                        .AllowAnyHeader());    // Allow all headers
            });



            builder.Services.AddDbContext<MyDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHostedService<SetWinnerBackgroundService>();
            builder.Services.AddScoped<SettWinner>();
            builder.Services.AddScoped<CreatePollWeekly>();


            builder.Services.AddScoped<WeeklyPollBackgroundService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
