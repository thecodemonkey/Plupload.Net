using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plupload.Net
{
    /// <summary>
    /// a transport object from server to the client. 
    /// This object will be filled on server, serialized as json and transported to the client.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// gets or sets the custom type of the message.
        /// </summary>
        public String MessageType { get; set; }

        /// <summary>
        /// gets or sets of the text. it can be error messages or something different.
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// gets or sets the thumbnail path of the uploaded image
        /// </summary>
        public String ThumbPath { get; set; }

        /// <summary>
        /// gets or sets the path of the uploaded image
        /// </summary>
        public String ImagePath { get; set; }

        /// <summary>
        /// gets or sets the name of the uploaded image
        /// </summary>
        public String FileName { get; set; }
    }
}
