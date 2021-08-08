using LightningAlert.BAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace LightningAlert.BAL
{
    public class LightningService : ILightningService
    {
        private readonly IStrikeManager _lightningManager;
        private readonly ICoordinatesManager _coordManager;
        private readonly IAssetManager _assetManager;
        private readonly IAlertManager _alertManager;

        public LightningService(IStrikeManager lightningManager,
                                ICoordinatesManager coordManager,
                                IAssetManager assetManager,
                                IAlertManager alertManager)
        {
            _lightningManager = lightningManager;
            _coordManager = coordManager;
            _assetManager = assetManager;
            _alertManager = alertManager;
        }

        public async Task StartMonitoring()
        {
            var tasks = new List<Task>();
            var strikes = _lightningManager.GetStrikesAsync();

            await foreach (var strike in strikes)
            {
                var task = Task.Run(() =>
                    {
                        try
                        {
                            _lightningManager.FilterStrike(strike);
                            var quadKey = _coordManager.GetQuadKey(strike.Latitude, strike.Longitude);
                            var asset = _assetManager.GetAssetByQuadKey(quadKey);
                            _alertManager.PopulateAlert(asset);
                        }
                        catch (Exception)
                        {
                            //TODO add logging
                        }
                    }
                );
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
        }
    }
}
