using MKS.Mobile.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MKS.Mobile.Core.Interfaces
{
    public interface IEmailService
    {
        Task<EmailResult> SendFile(Stream file, string fileName, string fileType, string reciprientEmail, string subject);

        Task<EmailResult> SendFile(string filePath, string fileType, string reciprientEmail, string subject);
        Task<EmailResult> SendFiles(IEnumerable<string> filePaths, string reciprientEmail, string subject);
    }
}
