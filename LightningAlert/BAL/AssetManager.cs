using LightningAlert.BAL.Interfaces;
using LightningAlert.DAL.Interfaces;
using LightningAlert.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;

namespace LightningAlert.BAL
{
    public class AssetManager : IAssetManager
    {
        private readonly IDataProvider<Asset> _dataProvider;
        private Dictionary<string, Asset> assets;
        private readonly object _lockObj = new object();

        public Dictionary<string, Asset> Assets
        {
            get
            {
                var lockTaken = false;
                Monitor.Enter(_lockObj, ref lockTaken);

                try
                {
                    if (assets == null)
                    {
                        assets = new Dictionary<string, Asset>();
                        using var stream = _dataProvider.GetStream();
                        string line;
                        List<Asset> assetsList = new List<Asset>();
                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                        while ((line = stream.ReadLine()) != null)
                        {
                            assetsList = JsonSerializer.Deserialize<List<Asset>>(line, options);
                        }

                        foreach (var asset in assetsList)
                            assets[asset.QuadKey] = asset;
                    }
                }
                catch(JsonException ex)
                {
                    //add logging;
                    throw;
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(_lockObj);
                }

                return assets;
            }
        }

        public AssetManager(IDataProvider<Asset> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public Asset GetAssetByQuadKey(string quadKey)
        {
            if (!Assets.ContainsKey(quadKey))
            {
                throw new Exception("Asset not found!");
            }

            return Assets[quadKey];
        }
    }
}
