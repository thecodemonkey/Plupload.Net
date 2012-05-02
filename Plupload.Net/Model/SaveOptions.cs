
namespace Plupload.Net.Model
{

    /// <summary>
    /// save options for uploaded files
    /// </summary>
    public enum SaveOptions
    {
        /// <summary>
        /// default saveoptions
        /// </summary>
        None,
        /// <summary>
        /// Override files when file name exists in the upload directory
        /// </summary>
        Override,
        /// <summary>
        /// Cancels Upload when file name exists in the upload directory
        /// </summary>
        CancelIfExists,
        /// <summary>
        /// Renames the file when file name exists in the upload directory
        /// </summary>
        RenameIfExists
    }
}