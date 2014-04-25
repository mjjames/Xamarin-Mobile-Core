using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using MKS.Mobile.Core.Interfaces;

namespace MKS.Mobile.Core.iOS.Services
{
    /// <summary>
    /// Wrapper File Storage Service that for GetFile ensures if the file is an image that the file is never bigger than a provided size
    /// </summary>
    public class ResizeImagesFileStorageService : IFileStorageService
    {
        private IFileStorageService _storageService;
        private SizeF _maxSize;
        public ResizeImagesFileStorageService(IFileStorageService underlyingStorageService, SizeF maxSize)
        {
            _storageService = underlyingStorageService;
            _maxSize = maxSize;
        }

        public async Task<Stream> GetFile(string path)
        {
            var file = await _storageService.GetFile(path);
            if (!path.EndsWith(".jpg"))
            {
                return file;
            }
            using (file)
            {
                using (var image = UIImage.LoadFromData(NSData.FromStream(file)))
                {
                    if (image.Size.Width > _maxSize.Width || image.Size.Height > _maxSize.Height)
                    {
                        using (var resizedImage = image.Scale(_maxSize))
                        {
                            return resizedImage.AsJPEG(0.85f).AsStream();
                        }
                    }
                    else
                    {
                        return image.AsJPEG(0.85f).AsStream();
                    }
                }
            }
        }

        public Task<string> PersistFile(Stream file, string fileName)
        {
            return _storageService.PersistFile(file, fileName);
        }

        public Task DeleteFile(string path)
        {
            return _storageService.DeleteFile(path);
        }
    }
}