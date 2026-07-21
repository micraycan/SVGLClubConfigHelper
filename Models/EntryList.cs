using System;
using System.Collections.Generic;
using System.Text;

namespace SVGLClubConfigHelper.Models
{
    public class EntryList
    {
        public List<CarEntry> Entries { get; set; }

        public EntryList(List<CarEntry> entries)
        {
            Entries = entries;
        }
    }
}
