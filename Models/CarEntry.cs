using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SVGLClubConfigHelper.Models
{
    public class CarEntry
    {
        public Car Car { get; set; }
        public string SelectedSkin { get; set; } = string.Empty;
        public int Ballast { get; set; } // adds weight to car
        public int Restrictor { get; set; } // reduce air going into engine/less power output

        public CarEntry(Car car)
        {
            Car = car;
            SelectedSkin = car.Skins.FirstOrDefault().SkinId;
            Ballast = 0;
            Restrictor = 0;
        }
    }
}
