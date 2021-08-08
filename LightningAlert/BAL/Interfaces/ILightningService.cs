using System.Threading.Tasks;

namespace LightningAlert.BAL.Interfaces
{
    public interface ILightningService
    {
        Task StartMonitoring();
    }
}
