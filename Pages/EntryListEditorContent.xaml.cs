using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.ApplicationModel.DataTransfer;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SVGLClubConfigHelper.ViewModels;
using SVGLClubConfigHelper.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Threading.Tasks;
using WinRT;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SVGLClubConfigHelper.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class EntryListEditorContent : Page
{
    public EntryListEditorViewModel ViewModel { get; }

    public EntryListEditorContent()
    {
        InitializeComponent();
        ViewModel = App.Host.Services.GetRequiredService<EntryListEditorViewModel>();
        DataContext = ViewModel;

        Loaded += OnLoaded;
    }

    private void OnLoaded(Object sender, RoutedEventArgs args)
    {
        ViewModel.InitializePage();
    }

    private void CarListView_DragItemsStarting(object sender, DragItemsStartingEventArgs args)
    {
        var car = (Car)args.Items[0];

        args.Data.SetText(car.CarId);
        args.Data.RequestedOperation = DataPackageOperation.Copy;
    }

    private void CarEntryListView_DragOver(object sender, DragEventArgs args)
    {
        args.AcceptedOperation = DataPackageOperation.Copy;
    }

    private async void CarEntryListView_Drop(object sender, DragEventArgs args)
    {
        if (!args.DataView.Contains(StandardDataFormats.Text)) { return; }

        var carId = await args.DataView.GetTextAsync();

        var car = ViewModel.CarList.FirstOrDefault(c => c.CarId == carId);

        if (car != null)
        {
            ViewModel.AddCarToEntryList(car);
        }
    }

    private void CarEntryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (sender is ComboBox comboBox 
            && comboBox.DataContext is CarEntry carEntry 
            && args.AddedItems.FirstOrDefault() is Skin skin)
        {
            ViewModel.SetCarEntrySkin(carEntry, skin.SkinId);
        }
    }

    private void CarEntryComboBox_OnLoaded(object sender, RoutedEventArgs args)
    {
        if (sender is ComboBox comboBox
            && comboBox.DataContext is CarEntry carEntry)
        {
            comboBox.SelectedIndex = carEntry.Car.Skins.FindIndex(s => s.SkinId == carEntry.SelectedSkin);
        }
    }

    private void BallastTextBox_OnLoaded(object sender, RoutedEventArgs args)
    {
        if (sender is TextBox textBox
            && textBox.DataContext is CarEntry carEntry)
        {
            textBox.Text = carEntry.Ballast.ToString();
        }
    }

    private void RestrictorTextBox_OnLoaded(object sender, RoutedEventArgs args)
    {
        if (sender is TextBox textBox
            && textBox.DataContext is CarEntry carEntry)
        {
            textBox.Text = carEntry.Restrictor.ToString();
        }
    }

    private void BallastTextBox_OnTextChanged(object sender, TextChangedEventArgs args)
    {
        if (sender is TextBox textBox
            && textBox.DataContext is CarEntry carEntry)
        {
            string input = textBox.Text.ToString();
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out int parsedInput))
                {
                    ViewModel.SetCarEntryBallast(carEntry, parsedInput);
                }
            }

            else
            {
                ViewModel.SetCarEntryBallast(carEntry, 0);
            }
        }
    }

    private void RestrictorTextBox_OnTextChanged(object sender, TextChangedEventArgs args)
    {
        if (sender is TextBox textBox
            && textBox.DataContext is CarEntry carEntry)
        {
            string input = textBox.Text.ToString();
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out int parsedInput))
                {
                    ViewModel.SetCarEntryRestrictor(carEntry, parsedInput);
                }
            }
            else
            {
                ViewModel.SetCarEntryRestrictor(carEntry, 0);
            }
        }
    }
}
