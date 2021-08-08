using LightningAlert.DAL.Interfaces;
using System.IO;

namespace LightningAlert.DAL
{
    public class DataProvider<T> : IDataProvider<T>
    {
        private readonly string _filePath;

        public DataProvider(string filePath)
        {
            _filePath = filePath;
        }
        public StreamReader GetStream()
        {
            return new StreamReader(_filePath);
        }
    }
}
