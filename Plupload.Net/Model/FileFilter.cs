using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Plupload.Net.Model
{
    [Serializable]
    public class FileFilter
    {
        [XmlAttribute("title")]
        public string title { get; set; }
        [XmlAttribute("extensions")]
        public string extensions { get; set; }

        public FileFilter Merge(FileFilter fileFilter) 
        {
            if (fileFilter == null) return this;

            FileFilter merged = new FileFilter();
            merged.title = (fileFilter.title != null) ? fileFilter.title : this.title;
            merged.extensions = (fileFilter.extensions != null) ? fileFilter.extensions : this.extensions;

            return merged;
        }
    }
}
