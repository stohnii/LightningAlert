using System.IO;

namespace LightningAlert.DAL.Interfaces
{
    public interface IDataProvider<T>
    {
        StreamReader GetStream();
    }
}
