using SVGLClubConfigHelper.Models;
using SVGLClubConfigHelper.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace SVGLClubConfigHelper.Services
{
    public class ContentLibraryService : IContentLibraryService
    {
        private Content _cachedContent;

        public Content LoadedContent => _cachedContent;

        public async Task<bool> TryLoadContent()
        {
            LocalFileSerializer serializer = new();
            _cachedContent = await serializer.GetFile<Content>("ContentCache.json");

            return _cachedContent != null;
        }
    }
    
    public interface IContentLibraryService
    {
        Content LoadedContent { get; }
        Task<bool> TryLoadContent();
    }
}
