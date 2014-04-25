using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.MessageUI;
using MonoTouch.UIKit;
using MKS.Mobile.Core.Interfaces;
using MKS.Mobile.Core.Models;

namespace MKS.Mobile.Core.iOS.Services
{
    public class iOSEmailService : IEmailService
    {
        private readonly IFileStorageService _fileStorageService;
        
        public iOSEmailService(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        public UIViewController ParentController { get; set; }

        public delegate Task FinishedDelegate(object sender, MFComposeResultEventArgs args);

        public async Task<EmailResult> SendFile(Stream file, string fileName , string fileType, string reciprientEmail, string subject)
        {
            
            var controller = new MFMailComposeViewController();
            try
            {
                controller.SetToRecipients(new[] {reciprientEmail});
                file.Position = 0;
                controller.AddAttachmentData(NSData.FromStream(file), fileType, fileName);
                var tsc = new TaskCompletionSource<EmailResult>();
                EventHandler<MFComposeResultEventArgs> finished = (obj, args) =>  ControllerOnFinished(tsc, args);
                controller.Finished += finished;
                ParentController.PresentViewController(controller, true, null);

                var result = await tsc.Task;
                controller.Finished -= finished;
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Email Controller Exception: " + ex.Message);
                throw;
            }
            finally
            {
                controller.Dispose();
            }
        }

        private void ControllerOnFinished(TaskCompletionSource<EmailResult> tsc, MFComposeResultEventArgs args)
        {
            if (args.Error != null)
            {
                tsc.SetException(new ApplicationException(args.Error.LocalizedDescription));
            }
            switch (args.Result)
            {
                case MFMailComposeResult.Cancelled:
                    tsc.SetResult(EmailResult.Cancelled);
                    break;
                case MFMailComposeResult.Saved:
                    tsc.SetResult(EmailResult.SavedToDrafts);
                    break;
                case MFMailComposeResult.Sent:
                    tsc.SetResult(EmailResult.Sent);
                    break;
                case MFMailComposeResult.Failed:
                default:
                    tsc.SetResult(EmailResult.FailedToSend);
                    break;
            }
            ParentController.DismissViewController(true, null);
        }

        public async Task<EmailResult> SendFile(string filePath, string fileType, string reciprientEmail, string subject)
        {
            using (var attachment = await _fileStorageService.GetFile(filePath))
            {
                return await SendFile(attachment, Path.GetFileName(filePath), fileType, reciprientEmail, subject);
            }
        }

        public async Task<EmailResult> SendFiles(IEnumerable<string> filePaths, string reciprientEmail, string subject)
        {
            var controller = new MFMailComposeViewController();
            try
            {
                controller.SetToRecipients(new[] { reciprientEmail });
                controller.SetSubject(subject);
                foreach (var filePath in filePaths)
                {
                    using (var data = await GetFile(filePath))
                    {
                        controller.AddAttachmentData(data, GetFileType(filePath), GetFileName(filePath));
                    }
                }
                
                var tsc = new TaskCompletionSource<EmailResult>();
                EventHandler<MFComposeResultEventArgs> finished = (obj, args) => ControllerOnFinished(tsc, args);
                controller.Finished += finished;
                ParentController.PresentViewController(controller, true, null);

                var result = await tsc.Task;
                controller.Finished -= finished;
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Email Controller Exception: " + ex.Message);
                throw;
            }
            finally
            {
                controller.Dispose();
            }
        }

        private async Task<NSData> GetFile(string path)
        {
            var file = await _fileStorageService.GetFile(path);
            return NSData.FromStream(file);
        }

        private string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        private string GetFileType(string filePath)
        {
            if (filePath.EndsWith(".txt"))
            {
                return "text/rtf";
            }
            if (filePath.EndsWith(".jpg"))
            {
                return "image/jpeg";
            }
            if (filePath.EndsWith(".pdf"))
            {
                return "application/x-pdf";
            }
            return "unknown";
        }
    }
}
