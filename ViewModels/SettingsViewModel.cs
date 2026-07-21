using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.Windows.Storage.Pickers;
using SVGLClubConfigHelper.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGLClubConfigHelper.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;
        private readonly IContentLoaderService _contentLoaderService;

        [ObservableProperty]
        public partial string ContentFolderPath { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool CanLoadContent { get; set; } = false;

        [ObservableProperty]
        public partial string LastLoadDate { get; set; } = string.Empty;

        public SettingsViewModel(ISettingsService settingsService, IContentLoaderService contentLoaderService)
        {
            _settingsService = settingsService;
            _contentLoaderService = contentLoaderService;

            ContentFolderPath = _settingsService.ContentFolderPath;
            CanLoadContent = _contentLoaderService.CanLoadContent();
            LastLoadDate = $"Last Updated: {_settingsService.LastContentLoadDate}";
        }

        [RelayCommand]
        private async Task BrowseContentFolder()
        {
            var folderPicker = new FolderPicker(App.MainWindow.AppWindow.Id);
            var directory = await folderPicker.PickSingleFolderAsync();

            if (directory != null)
            {
                ContentFolderPath = directory.Path;
                _settingsService.ContentFolderPath = directory.Path;
            }
        }

        [RelayCommand]
        private async Task LoadContentFolder()
        {
            await _contentLoaderService.LoadContent();
            _settingsService.LastContentLoadDate = DateTime.Now.ToShortDateString();
            LastLoadDate = $"Last Updated: {_settingsService.LastContentLoadDate}";
        }

        partial void OnContentFolderPathChanged(string value)
        {
            CanLoadContent = _contentLoaderService.CanLoadContent();
        }
    }
}