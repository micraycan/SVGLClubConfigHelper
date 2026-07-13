using System;
using System.Collections.Generic;
using System.Text;

namespace SVGLClubConfigHelper.Models
{
    public class Car
    {
        public string CarId { get; }
        public string Name { get; }
        public List<Skin> Skins { get; }
        public string BadgeImagePath { get; }
        public Specs Specs { get; }

        public Car(string carId, string name, List<Skin> skins, string badgeImagePath, Specs specs)
        {
            CarId = carId;
            Name = name;
            Skins = skins;
            BadgeImagePath = badgeImagePath;
            Specs = specs;
        }
    }
}
