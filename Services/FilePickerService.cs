using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.Windows.Storage.Pickers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SVGLClubConfigHelper.Services
{
    public class FilePickerService : IFilePickerService
    {
        private readonly WindowId _windowId;

        public FilePickerService(WindowId windowId)
        {
            _windowId = windowId;
        }

        public async Task<string?> PickFolderAsync()
        {
            var folderPicker = new FolderPicker(_windowId);
            var folder = await folderPicker.PickSingleFolderAsync();

            if (folder == null) { return null; }
            return folder.Path;
        }
    }

    public interface IFilePickerService
    {
        Task<string?> PickFolderAsync();
    }
}
