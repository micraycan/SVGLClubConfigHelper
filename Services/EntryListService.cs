using SVGLClubConfigHelper.Models;
using SVGLClubConfigHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace SVGLClubConfigHelper.Services
{
    public class EntryListService : IEntryListService
    {
        public async Task UpdateEntryListAsync(List<CarEntry> entries)
        {
            EntryList entryList = new(entries);

            LocalFileSerializer serializer = new();
            await serializer.SaveFile("EntryList.json", entryList);
        }

        public async Task<EntryList> GetEntryListAsync()
        {
            LocalFileSerializer serializer = new();
            return await serializer.GetFile<EntryList>("EntryList.json");
        }

        public string BuildEntryList(List<CarEntry> entries)
        {
            StringBuilder sb = new();
            int index = 0;

            foreach(var entry in entries)
            {
                sb.AppendLine($"[CAR_{index}]");
                sb.AppendLine($"MODEL={entry.Car.CarId}");
                sb.AppendLine($"SKIN={entry.SelectedSkin}");
                sb.AppendLine($"SPECTATOR_MODE=0");
                sb.AppendLine($"DRIVERNAME=");
                sb.AppendLine($"TEAM=");
                sb.AppendLine($"GUID=");
                sb.AppendLine($"BALLAST={entry.Ballast.ToString()}");
                sb.AppendLine($"RESTRICTOR={entry.Restrictor.ToString()}");
                sb.AppendLine($"");

                index++;
            }
            Debug.WriteLine(sb.ToString());
            return sb.ToString();
        }

        public string BuildCarSelection(List<CarEntry> entries)
        {
            StringBuilder sb = new();
            HashSet<string> carIds = new();

            foreach (var entry in entries)
            {
                string id = entry.Car.CarId;
                if (carIds.Add(id))
                {
                    sb.Append($"{id};");
                }
            }

            return sb.ToString();
        }
    }

    public interface IEntryListService
    {
        Task UpdateEntryListAsync(List<CarEntry> entries);
        Task<EntryList> GetEntryListAsync();
        string BuildEntryList(List<CarEntry> entries);
        string BuildCarSelection(List<CarEntry> entries);
    }
}
