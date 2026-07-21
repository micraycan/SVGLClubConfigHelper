using System;
using System.Collections.Generic;
using System.Text;

namespace SVGLClubConfigHelper.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly Windows.Storage.ApplicationDataContainer _localSettings;

        public SettingsService()
        {
            _localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        }

        public string ContentFolderPath
        {
            get
            {
                return _localSettings.Values["ContentFolderPath"] as string ?? string.Empty;
            }
            set
            {
                _localSettings.Values["ContentFolderPath"] = value;
            }
        }

        public string LastContentLoadDate
        {
            get
            {
                return _localSettings.Values["LastContentLoadDate"] as string ?? "01-01-2000";
            }
            set
            {
                _localSettings.Values["LastContentLoadDate"] = value;
            }
        }
    }

    public interface ISettingsService
    {
        string ContentFolderPath { get; set; }
        string LastContentLoadDate { get; set; }
    }
}
