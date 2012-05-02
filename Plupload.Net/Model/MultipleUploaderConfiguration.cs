using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plupload.Net.Model
{
    [Serializable]
    public  class MultipleUploaderConfiguration
    {
        public List<FileFilter> FileFilter { get; set; }

        public MultipleUploaderConfiguration()
        {
            this.FileFilter = new List<FileFilter>();
        }
    }
}
