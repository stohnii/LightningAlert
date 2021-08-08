using LightningAlert.BAL;
using LightningAlert.BAL.Interfaces;
using LightningAlert.DAL;
using LightningAlert.DAL.Interfaces;
using LightningAlert.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace LightningAlert
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string strikeFilePath = @"..\..\..\AppData\lightning.json";
            string assetFilePath = @"..\..\..\AppData\assets.json";

            //setup DI
            var serviceProvider = new ServiceCollection()
                .AddScoped<IMemoryCache>(x => new MemoryCache(new MemoryCacheOptions()))
                .AddScoped<ILightningService, LightningService>()
                .AddSingleton<IDataProvider<Strike>>(x => new DataProvider<Strike>(strikeFilePath))
                .AddSingleton<IDataProvider<Asset>>(x => new DataProvider<Asset>(assetFilePath))
                .AddScoped<IStrikeManager, StrikeManager>()
                .AddScoped<ICoordinatesManager, CoordinatesManager>()
                .AddSingleton<IAssetManager, AssetManager>()
                //.AddScoped<ICacheProvider, RedisCacheProvider>()
                .AddScoped<ICacheProvider, InMemoryCache>()
                .AddScoped<IAlertManager, ConsoleAlertManager>()
                .BuildServiceProvider();

            //do work
            var lightningService = serviceProvider.GetService<ILightningService>();
            await lightningService.StartMonitoring();
        }
    }
}
