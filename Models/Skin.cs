using System;
using System.Collections.Generic;
using System.Text;

namespace SVGLClubConfigHelper.Models
{
    public class Skin
    {
        public string SkinId { get; }
        public string PreviewPath { get; }

        public Skin(string skinId, string previewPath)
        {
            SkinId = skinId;
            PreviewPath = previewPath;
        }
    }
}
