using System.IO;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MKS.Mobile.Core.Interfaces;

namespace MKS.Mobile.Core.iOS.Services
{
    public class iOSFileStorageService : IFileStorageService
    {
        public Task<Stream> GetFile(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            return Task.FromResult<Stream>(File.OpenRead(path));
        }

        public async Task<string> PersistFile(Stream file, string fileName)
        {
            var filePath = fileName;
            var folderPath = Path.GetDirectoryName(fileName);
            //if our filename is just a filename with no path
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                folderPath = Path.GetTempPath();
                //store the file in temp and use the filename provided or create a new random one if none provided
                filePath = Path.Combine(folderPath, fileName ?? Path.GetRandomFileName());
            }

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            //create the new file and copy the stream into the file stream
            var retryPersist = false;
            try
            {
                using (var fileStream = File.OpenWrite(filePath))
                {
                    await file.CopyToAsync(fileStream);
                    //fileStream.Close();
                }
            }
            catch (IOException ex)
            {
                if (ex.Message.StartsWith("Sharing violation on path"))
                {
                    retryPersist = true;
                }
                else
                {
                    throw;
                }
            }
            if (retryPersist)
            {
                await Task.Delay(1000);
                filePath = await PersistFile(file, fileName);
            }
            return filePath;
        }

        public async Task DeleteFile(string path)
        {
            await Task.Factory.StartNew(() => File.Delete(path));
        }
    }
}
