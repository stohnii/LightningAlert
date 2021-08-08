using LightningAlert.Models;
using System.Collections.Generic;

namespace LightningAlert.BAL.Interfaces
{
    public interface IStrikeManager
    {
        IAsyncEnumerable<Strike> GetStrikesAsync();
        void FilterStrike(Strike strike);
    }
}
