using SVGLClubConfigHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SVGLClubConfigHelper.Utilities
{
    public class CarParser
    {
        private readonly string _carDir;

        public CarParser(string carDir)
        {
            _carDir = carDir;
        }

        public string GetCarName()
        {
            string uiPath = Path.Combine(_carDir, "ui", "ui_car.json");
            string fallbackName = Path.GetFileName(_carDir);
            if (!File.Exists(uiPath)) { return fallbackName; }

            string text = File.ReadAllText(uiPath);
            var match = Regex.Match(text, "\"name\"\\s*:\\s*\"(?<name>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (match.Success) { return match.Groups["name"].Value.Trim(); }
            else { return fallbackName; }
        }


        public Specs GetCarSpecs()
        {
            string uiPath = Path.Combine(_carDir, "ui", "ui_car.json");
            if (!File.Exists(uiPath)) { return new(); }

            string text = File.ReadAllText(uiPath);
            var bhpMatch = Regex.Match(text, "\"bhp\"\\s*:\\s*\"(?<bhp>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var torqueMatch = Regex.Match(text, "\"torque\"\\s*:\\s*\"(?<torque>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var weightMatch = Regex.Match(text, "\"weight\"\\s*:\\s*\"(?<weight>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var pwratioMatch = Regex.Match(text, "\"pwratio\"\\s*:\\s*\"(?<pwratio>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            string bhp = bhpMatch.Success ? bhpMatch.Groups["bhp"].Value.Trim() : "N/A";
            string torque = torqueMatch.Success ? torqueMatch.Groups["torque"].Value.Trim() : "N/A";
            string weight = weightMatch.Success ? weightMatch.Groups["weight"].Value.Trim() : "N/A";
            string pwratio = pwratioMatch.Success ? pwratioMatch.Groups["pwratio"].Value.Trim() : "N/A";

            return new Specs(bhp, torque, weight, pwratio);
        }

        public List<Skin> GetSkins()
        {
            List<Skin> skins = new();
            string skinsPath = Path.Combine(_carDir, "skins");

            foreach (var skinDir in Directory.GetDirectories(skinsPath))
            {
                if (!Directory.EnumerateFileSystemEntries(skinDir).Any()) { continue; }

                string skinId = Path.GetFileName(skinDir);
                string previewPath = Path.Combine(skinDir, "preview.jpg");

                if (!File.Exists(previewPath)) { continue; }

                skins.Add(new Skin(skinId, previewPath));
            }

            return skins;
        }
    }
}
