using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SVGLClubConfigHelper.Models
{
    public class Specs
    {
        public string BHP { get; }
        public string Torque { get; }
        public string Weight { get; }
        public string PWRatio { get; }

        public Specs() : this("N/A", "N/A", "N/A", "N/A") { }

        [JsonConstructor]
        public Specs(string bhp, string torque, string weight, string pwratio)
        {
            BHP = bhp;
            Torque = torque;
            Weight = weight;
            PWRatio = pwratio;
        }
    }
}
