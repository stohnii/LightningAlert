using LightningAlert.BAL.Interfaces;
using LightningAlert.Models;
using System;
using System.Threading;

namespace LightningAlert.BAL
{
    public class ConsoleAlertManager : IAlertManager
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly object _lockObj = new object();

        public ConsoleAlertManager(ICacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public void PopulateAlert(Asset asset)
        {
            var lockTaken = false;
            Monitor.Enter(_lockObj, ref lockTaken);

            try
            {
                if (!_cacheProvider.KeyExists(asset.QuadKey))
                {
                    _cacheProvider.AddKey(asset.QuadKey);
                    Console.WriteLine($"lightning alert for {asset.AssetOwner}:{asset.AssetName}");
                }
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(_lockObj);
            }
        }
    }
}
