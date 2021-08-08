using LightningAlert.BAL.Interfaces;
using StackExchange.Redis;
using System;

namespace LightningAlert.BAL
{
    public class RedisCacheProvider : ICacheProvider, IDisposable
    {
        private string config = "localhost:6379"; // TODO move to config
        private ConnectionMultiplexer connection;
        private IDatabase database;
        private string groupName = "AssetsQuadKeys"; // TODO move to method params

        public RedisCacheProvider()
        {
            connection = ConnectionMultiplexer.Connect(new ConfigurationOptions { AbortOnConnectFail = false, ConnectRetry = 5, EndPoints = { config } });
            database = connection.GetDatabase(-1);

            //clear cache
            database.KeyDelete($"{groupName}");
        }

        public void AddKey(string quadKey)
        {
            database.SetAdd($"{groupName}", new RedisValue(quadKey));
        }

        public bool KeyExists(string quadKey)
        {
            return database.SetContains($"{groupName}", $"{quadKey}");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection != null)
                {
                    connection.Close();
                    connection.Dispose();
                    connection = null;
                }
            }
        }
    }
}
