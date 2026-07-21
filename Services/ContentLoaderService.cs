using SVGLClubConfigHelper.Models;
using SVGLClubConfigHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace SVGLClubConfigHelper.Services
{
    public class ContentLoaderService : IContentLoaderService
    {
        private readonly ISettingsService _settingsService;

        public ContentLoaderService(ISettingsService settingService)
        {
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

        public async Task LoadContent()
        {
            List<Car> cars = await LoadCars();
            Content content = new(cars);

            LocalFileSerializer serializer = new();
            await serializer.SaveFile("ContentCache.json", content);
        }

        public async Task<List<Car>> LoadCars()
        {
            List<Car> cars = new();

            string contentPath = _settingsService.ContentFolderPath;
            if (!CanLoadContent()) { return cars; }

            string carPath = Path.Combine(contentPath, @"cars\");

            foreach (var carDir in Directory.GetDirectories(carPath))
            {
                if (!Directory.EnumerateFileSystemEntries(carDir).Any()) { continue; }

                CarParser parser = new(carDir);
                string carId = Path.GetFileName(carDir);
                string carName = parser.GetCarName();
                List<Skin> skins = parser.GetSkins();
                Specs specs = parser.GetCarSpecs();
                
                if (!skins.Any()) { continue; }

                string badgePath = Path.Combine(carDir, "logo.png");

                cars.Add(new Car(carId, carName, skins, badgePath, specs));
            }

            return cars;
        }
    }

    public interface IContentLoaderService
    {
        bool CanLoadContent();
        Task LoadContent();
        Task<List<Car>> LoadCars();
    }
}
