using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using SpeedtestVisualizer.Misc.Constants;
using SpeedtestVisualizer.Misc.Contexts;
using SpeedtestVisualizer.Misc.Services;

namespace SpeedtestVisualizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var syncfusionLicenseKey = builder.Configuration.GetValue<string>(AppSettingConstants.SyncfusionLicenseKey);
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicenseKey);
            
            var appDbConnectionString = builder.Configuration.GetConnectionString("AppDb");

            // Add services to the container.
            builder.Services.AddDbContext<SpeedtestVisualizerContext>(
                options => options.UseMySQL(appDbConnectionString));
            builder.Services.AddHostedService<SpeedtestService>();
            builder.Services.AddControllersWithViews();
            
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            var app = builder.Build();
            
            app.UseForwardedHeaders();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/Index");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
    
    public class LineChartData
    {
        public DateTime xValue;
        public double yValue;
        public double yValue1;
    }
}