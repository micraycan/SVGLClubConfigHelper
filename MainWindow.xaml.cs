using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SVGLClubConfigHelper.Pages;
using SVGLClubConfigHelper.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SVGLClubConfigHelper
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (AppWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.Maximize();
            }
        }

        private void MainNav_Loaded(object sender, RoutedEventArgs e)
        {
            MainNav.SelectedItem = MainNav.MenuItems[0];
        }

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem item)
            {
                switch (item.Tag)
                {
                    case "EntryListEditor":
                        ContentFrame.Navigate(typeof(EntryListEditorContent));
                        break;
                    case "TrackSelection":
                        ContentFrame.Navigate(typeof(TrackSelectionContent));
                        break;
                    case "ModUpdater":
                        ContentFrame.Navigate(typeof(ModUpdaterContent));
                        break;
                }
            }

            if (args.IsSettingsSelected)
            {
                ContentFrame.Navigate(typeof(SettingsContent));
                return;
            }
        }
    }
}
