using System;
using System.Collections.Generic;
using System.Text;

namespace LightningAlert.BAL.Interfaces
{
    public interface ICoordinatesManager
    {
        string GetQuadKey(double latitude, double longitude);
    }
}
