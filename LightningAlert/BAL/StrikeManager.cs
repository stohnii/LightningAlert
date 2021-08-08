using LightningAlert.BAL.Interfaces;
using LightningAlert.DAL.Interfaces;
using LightningAlert.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LightningAlert.BAL
{
    public class StrikeManager : IStrikeManager
    {
        private readonly IDataProvider<Strike> _dataProvider;

        public StrikeManager(IDataProvider<Strike> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public async IAsyncEnumerable<Strike> GetStrikesAsync()
        {
            var line = string.Empty;
            Strike strike;
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            using var stream = _dataProvider.GetStream();
            while ((line = await stream.ReadLineAsync()) != null)
            {
                try
                {
                    strike = JsonSerializer.Deserialize<Strike>(line, options);
                }
                catch (JsonException)
                {
                    throw;
                }
                
                yield return strike;
            }
        }

        public void FilterStrike(Strike strike)
        {
            if (strike.FlashType == FlashType.HeartBeat)
            {
                throw new Exception("HeartBeat!");
            }
        }
    }
}
