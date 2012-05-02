using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Plupload.Net.Model
{
    /// <summary>
    /// defines property for automatic resizing of uploaded files
    /// </summary>
    public class ResizeProperty
    {
        public ResizeProperty() { }

        public ResizeProperty(int width, int height, int quality) 
        {
            this.width = width;
            this.height = height;
            this.quality = quality;
        }

        /// <summary>
        /// gets or sets the target width of the uploaded file
        /// </summary>
        public int? width { get; set; }

        /// <summary>
        /// gets or sets the target height of the uploaded file
        /// </summary>
        public int? height { get; set; }

        /// <summary>
        /// gets or sets the quality of the uploaded file
        /// </summary>
        public int? quality { get; set; }

        /// <summary>
        /// gets or sets the flag if the ratio of the Image should be aspected.
        /// </summary>
        public bool? AspectRatio { get; set; }


        /// <summary>
        /// merges the current instance of the ResizeProperty with the specific instance given by resize.
        /// </summary>
        /// <param name="resize">a specific instance of the ResizeProperty wich should be merged with the current instance</param>
        /// <returns>merged result</returns>
        public ResizeProperty Merge(ResizeProperty resize)
        {
            if (resize == null) return this;

            ResizeProperty merged = new ResizeProperty();
            merged.width = (resize.width.HasValue)? resize.width : this.width;
            merged.height = (resize.height.HasValue) ? resize.height : this.height;
            merged.quality = (resize.quality.HasValue) ? resize.quality : this.quality;
            merged.AspectRatio = (resize.AspectRatio.HasValue) ? resize.AspectRatio : this.AspectRatio;
            return merged;
        }
    }
}
