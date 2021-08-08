using LightningAlert.Models;

namespace LightningAlert.BAL.Interfaces
{
    public interface IAlertManager
    {
        void PopulateAlert(Asset asset);
    }
}
