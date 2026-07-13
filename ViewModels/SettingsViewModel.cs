using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SVGLClubConfigHelper.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGLClubConfigHelper.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly IFilePickerService _filePickerService;
        private readonly ISettingsService _settingsService;

        public SettingsViewModel(IFilePickerService filePickerService, ISettingsService settingsService)
        {
            _filePickerService = filePickerService;
            _settingsService = settingsService;

            ContentFolderPath = _settingsService.ContentFolderPath;
        }

        [ObservableProperty]
        public partial string ContentFolderPath { get; set; } = String.Empty;

        [RelayCommand]
        private async Task BrowseContentFolder()
        {
            var directory = await _filePickerService.PickFolderAsync();

            if (directory != null)
            {
                ContentFolderPath = directory;
                _settingsService.ContentFolderPath = directory;
            }
        }
    }
}