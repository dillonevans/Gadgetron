using Gadgetron.Ps2;
using Gadgetron.RatchetAndClank;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Gadgetron
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Services.AddSerilog();
            builder.Services.AddScoped<Pcsx2Client>();
            builder.Services.AddScoped<Trainer>();
            
            var host = builder.Build();

            var trainer = host.Services.GetRequiredService<Trainer>();
            await trainer.RunAsync();
        }
    }
}
