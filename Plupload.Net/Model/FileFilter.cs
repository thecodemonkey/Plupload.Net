using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Plupload.Net.Model
{
    /// <summary>
    /// defines a single filter of allowed files, wich can be uploaded
    /// </summary>
    [Serializable]
    public class FileFilter
    {
        /// <summary>
        /// gets or sets the Title of the filefilter. Displays within "selct files" Dialog.
        /// </summary>
        [XmlAttribute("title")]
        public string title { get; set; }

        /// <summary>
        /// gets or sets the allowed file extensions. For example: "jpg,jpeg,png,gif,tiff,ico"
        /// </summary>
        [XmlAttribute("extensions")]
        public string extensions { get; set; }

        /// <summary>
        /// merges the current instance of the FileFilter with the specific instance given by fileFilter.
        /// </summary>
        /// <param name="fileFilter">a specific instance of the FileFilter wich should be merged with the current instance</param>
        /// <returns>merged result</returns>
        public FileFilter Merge(FileFilter fileFilter) 
        {
            if (fileFilter == null) return this;

            FileFilter merged = new FileFilter();
            merged.title = (fileFilter.title != null) ? fileFilter.title : this.title;
            merged.extensions = (fileFilter.extensions != null) ? fileFilter.extensions : this.extensions;

            return merged;
        }
    }

    /*
    public static class FileFilterExtensions 
    {
        public static List<FileFilter> Merge(this List<FileFilter> fileFilters, List<FileFilter> newFileFilters) 
        {
            if (newFileFilters == null)
            
            if (fileFilters == null || fileFilters.Count == 0) return newFileFilters;

            
        }
    }*/
}
