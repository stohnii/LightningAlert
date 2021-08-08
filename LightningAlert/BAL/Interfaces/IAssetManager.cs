using LightningAlert.Models;

namespace LightningAlert.BAL.Interfaces
{
    public interface IAssetManager
    {
        Asset GetAssetByQuadKey(string quadKey);
    }
}
