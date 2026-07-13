using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SVGLClubConfigHelper.Models;
using SVGLClubConfigHelper.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVGLClubConfigHelper.ViewModels
{
    public partial class EntryListEditorViewModel : ObservableObject
    {
        private readonly IContentLoaderService _contentLoaderService;

        [ObservableProperty]
        public partial List<Car> CarList { get; set; } = new();

        [ObservableProperty]
        public partial Visibility CarListVisible { get; set; } = Visibility.Collapsed;

        [ObservableProperty]
        public partial Visibility ProgressBarVisible { get; set; } = Visibility.Visible;

        [ObservableProperty]
        public partial bool CarListLoadingError { get; set; } = false;

        [ObservableProperty]
        public partial string CarListSearchBar { get; set; } = string.Empty;

        [ObservableProperty]
        public partial Car? SelectedCar { get; set; } = null;

        [ObservableProperty]
        public partial bool IsCarSelected { get; set; } = false;

        [ObservableProperty]
        public partial List<Skin> AvailableSkins { get; set; } = new();

        [ObservableProperty]
        public partial Skin? SelectedSkin { get; set; } = null;

        [ObservableProperty]
        public partial ImageSource? SkinPreview { get; set; } = null;

        private List<Car> _cachedList = new();

        public EntryListEditorViewModel(IContentLoaderService contentLoaderService)
        {
            _contentLoaderService = contentLoaderService;
        }

        public async Task InitializeAsync()
        {
            CarListLoadingError = !_contentLoaderService.CanLoadContent();

            if (CarListLoadingError) { return; }

            await LoadContent();
        }

        private async Task LoadContent()
        {
            var list = await _contentLoaderService.LoadCars();

            CarList = list.OrderBy(c => c.Name).ToList();
            _cachedList = CarList;

            ProgressBarVisible = Visibility.Collapsed;
            CarListVisible = Visibility.Visible;
        }

        partial void OnCarListSearchBarChanged(string value)
        {
            if (String.IsNullOrEmpty(value) || String.IsNullOrWhiteSpace(value))
            {
                CarList = _cachedList;
                return;
            }

            var list = _cachedList
                .Where(c => c.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name);

            CarList = list.ToList();
        }

        partial void OnSelectedCarChanged(Car? value)
        {
            IsCarSelected = value != null;

            if (value != null)
            {
                AvailableSkins = value.Skins;
                SelectedSkin = AvailableSkins.FirstOrDefault();
            }
        }

        partial void OnSelectedSkinChanged(Skin? value)
        {
            if (value != null)
            {
                Image img = new();
                BitmapImage bitmap = new();
                Uri uri = new Uri(value.PreviewPath);
                bitmap.UriSource = uri;
                SkinPreview = bitmap;
            }
        }
    }
}
