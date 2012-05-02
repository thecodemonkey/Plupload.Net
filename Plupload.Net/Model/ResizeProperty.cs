using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Plupload.Net.Model
{
    public class ResizeProperty
    {
        public ResizeProperty() { }

        public ResizeProperty(int width, int height, int quality) 
        {
            this.width = width;
            this.height = height;
            this.quality = quality;
        }

        //[XmlAttribute("width")]
        public int? width { get; set; }
        //[XmlAttribute("height")]
        public int? height { get; set; }
        //[XmlAttribute("quality")]
        public int? quality { get; set; }
        //[XmlAttribute("aspectRatio")]
        public bool? AspectRatio { get; set; }


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
