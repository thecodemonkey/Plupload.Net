
namespace Plupload.Net.Model
{

    public enum SaveOptions
    {
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