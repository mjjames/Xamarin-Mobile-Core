using System.IO;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IFileStorageService
    {
        /// <summary>
        /// Retrieves the file at the supplied path for reading
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Stream for the file</returns>
        Task<Stream> GetFile(string path);
        /// <summary>
        /// Persists the file stream to storage using the provided filename
        /// </summary>
        /// <param name="file">Stream of file to persist</param>
        /// <param name="fileName">Name for the file</param>
        /// <returns>Fullpath to the file</returns>
        Task<string> PersistFile(Stream file, string fileName);

        Task DeleteFile(string path);
    }
}
