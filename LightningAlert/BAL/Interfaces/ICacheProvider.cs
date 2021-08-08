namespace LightningAlert.BAL.Interfaces
{
    public interface ICacheProvider
    {
        bool KeyExists(string quadKey);
        void AddKey(string quadKey);
    }
}
