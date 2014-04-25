using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using System.IO;
using MKS.Mobile.Core.Interfaces;

namespace MKS.Mobile.Core.iOS
{
    public class iOSImageHelper : IImageHelper
    {
        private IFileStorageService _fileStorage;
        public iOSImageHelper(IFileStorageService fileStorage)
        {
            _fileStorage = fileStorage;
        }
        public string ToBase64Jpeg(string path)
        {
            //todo: can we await this?
            using (var file = _fileStorage.GetFile(path).Result)
            {
                using (var data = NSData.FromStream(file))
                {
                    return string.Format("data:image/jpeg;base64,{0}", data.GetBase64EncodedString(NSDataBase64EncodingOptions.None));
                }
            }
           
        }
    }
}