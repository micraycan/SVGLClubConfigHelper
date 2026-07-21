using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace SVGLClubConfigHelper.Utilities
{
    public class LocalFileSerializer
    {
        public async Task<T> GetFile<T>(string fileName)
        {
            string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, fileName);

            if (!File.Exists(path)) { return default; }

            string json = await File.ReadAllTextAsync(path);

            return JsonSerializer.Deserialize<T>(json);
        }

        public async Task SaveFile<T>(string fileName, T obj)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

            await FileIO.WriteTextAsync(file, json);
        }
    }
}
