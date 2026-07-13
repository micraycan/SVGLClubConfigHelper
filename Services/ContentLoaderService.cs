using SVGLClubConfigHelper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SVGLClubConfigHelper.Services
{
    public class ContentLoaderService : IContentLoaderService
    {
        private readonly IFilePickerService _filePickerService;
        private readonly ISettingsService _settingsService;

        public ContentLoaderService(IFilePickerService filePickerService, ISettingsService settingService)
        {
            _filePickerService = filePickerService;
            _settingsService = settingService;
        }

        public bool CanLoadContent()
        {
            string contentPath = _settingsService.ContentFolderPath;
            if (contentPath == string.Empty) { return false; }

            string carPath = Path.Combine(contentPath, @"cars\");
            string trackPath = Path.Combine(contentPath, @"tracks\");

            return Directory.Exists(carPath) && Directory.Exists(trackPath);
        }

        public async Task<List<Car>> LoadCars()
        {
            List<Car> cars = new();
            if (!CanLoadContent()) { return cars; }

            string contentPath = _settingsService.ContentFolderPath;
            string carPath = Path.Combine(contentPath, @"cars\");

            foreach (var carDir in Directory.GetDirectories(carPath))
            {
                if (!Directory.EnumerateFileSystemEntries(carDir).Any()) { continue; }

                string carId = Path.GetFileName(carDir);
                string carName = GetCarName(carDir);
                string skinsPath = Path.Combine(carDir, "skins");
                List<Skin> skins = GetSkins(skinsPath);
                Specs specs = GetCarSpecs(carDir);
                
                if (!skins.Any()) { continue; }

                string badgePath = Path.Combine(carDir, "logo.png");

                cars.Add(new Car(carId, carName, skins, badgePath, specs));
            }

            return cars;
        }

        private string GetCarName(string carDir)
        {
            string uiPath = Path.Combine(carDir, "ui", "ui_car.json");
            string fallbackName = Path.GetFileName(carDir);
            if (!File.Exists(uiPath)) { return fallbackName; }

            string text = File.ReadAllText(uiPath);
            var match = Regex.Match(text, "\"name\"\\s*:\\s*\"(?<name>.*?)\"", RegexOptions.IgnoreCase | RegexOptions.Singleline);

            if (match.Success) { return match.Groups["name"].Value.Trim(); }
            else { return fallbackName; }
        }

        private List<Skin> GetSkins(string skinsPath)
        {
            List<Skin> skins = new();

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

        private Specs GetCarSpecs(string carDir)
        {
            string uiPath = Path.Combine(carDir, "ui", "ui_car.json");
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
    }

    public interface IContentLoaderService
    {
        bool CanLoadContent();
        Task<List<Car>> LoadCars();
    }
}
