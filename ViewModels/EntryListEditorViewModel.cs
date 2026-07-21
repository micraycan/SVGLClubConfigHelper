using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SVGLClubConfigHelper.Models;
using SVGLClubConfigHelper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace SVGLClubConfigHelper.ViewModels
{
    public partial class EntryListEditorViewModel : ObservableObject
    {
        private readonly IContentLibraryService _contentLibraryService;
        private readonly IEntryListService _entryListService;

        [ObservableProperty]
        public partial bool CarListLoadingError { get; set; } = false;

        [ObservableProperty]
        public partial List<Car> CarList { get; set; } = new();

        [ObservableProperty]
        public partial ObservableCollection<CarEntry> EntryList { get; set; } = new();

        [ObservableProperty]
        public partial string CarSearchBar { get; set; } = string.Empty;

        [ObservableProperty]
        public partial CarEntry? SelectedCarEntry { get; set; }

        [ObservableProperty]
        public partial bool IsCarEntrySelected { get; set; } = false;

        [ObservableProperty]
        public partial string EntryListString { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string CarSelectionString { get; set; } = string.Empty;

        private List<Car> _cachedCarList = new();
        private EntryList? _entryList;

        public EntryListEditorViewModel(IContentLibraryService contentLibraryService, IEntryListService entryListService)
        {
            _contentLibraryService = contentLibraryService;
            _entryListService = entryListService;
        }

        public async Task InitializePage()
        {
            CarListLoadingError = !await _contentLibraryService.TryLoadContent();

            if (CarListLoadingError) { return; }
            CarList = _contentLibraryService.LoadedContent.Cars;
            _cachedCarList = CarList;

            _entryList = await _entryListService.GetEntryListAsync();
            if (_entryList is null) { return; }
            EntryList = new(_entryList.Entries);
            UpdateEntryListString();
        }

        partial void OnCarSearchBarChanged(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                CarList = _cachedCarList;
                return;
            }

            var list = _cachedCarList
                .Where(c => c.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name);

            CarList = list.ToList();
        }

        public void AddCarToEntryList(Car car)
        {
            EntryList.Add(new(car));
            UpdateEntryList();
        }

        [RelayCommand]
        public void DeleteCarEntry(CarEntry car)
        {
            EntryList.Remove(car);
            UpdateEntryList();
        }

        [RelayCommand]
        public void CopyEntryList()
        {
            DataPackage data = new();
            data.RequestedOperation = DataPackageOperation.Copy;
            data.SetText(EntryListString);
            Clipboard.SetContent(data);
        }

        [RelayCommand]
        public void CopyCarSelection()
        {
            DataPackage data = new();
            data.RequestedOperation = DataPackageOperation.Copy;
            data.SetText(CarSelectionString);
            Clipboard.SetContent(data);
        }

        public void SetCarEntrySkin(CarEntry carEntry, string skinId)
        {
            foreach (var entry in EntryList)
            {
                if (entry != carEntry) { continue; }
                entry.SelectedSkin = skinId;
            }

            UpdateEntryList();
        }

        public void SetCarEntryBallast(CarEntry carEntry, int ballast)
        {
            foreach (var entry in EntryList)
            {
                if (entry != carEntry) { continue; }
                entry.Ballast = ballast;
            }

            UpdateEntryList();
        }

        public void SetCarEntryRestrictor(CarEntry carEntry, int restrictor)
        {
            foreach (var entry in EntryList)
            {
                if (entry != carEntry) { continue; }
                entry.Restrictor = restrictor;
            }

            UpdateEntryList();
        }

        private void UpdateEntryList()
        {
            _entryListService.UpdateEntryListAsync(EntryList.ToList());
            UpdateEntryListString();
        }

        private void UpdateEntryListString()
        {
            EntryListString = _entryListService.BuildEntryList(EntryList.ToList());
            CarSelectionString = _entryListService.BuildCarSelection(EntryList.ToList());
        }
    }
}
