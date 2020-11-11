using Distvisor.Web.Data.Events;
using Distvisor.Web.Data.Events.Core;
using Microsoft.AspNetCore.Http;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distvisor.Web.Services
{
    public interface IEmailFileImportService
    {
        Task ImportEmailFilesAsync(IEnumerable<IFormFile> files);
    }

    public class EmailFileImportService : IEmailFileImportService
    {
        private readonly IEventStore _events;
        private readonly INotificationService _notifications;

        public EmailFileImportService(IEventStore eventStore, INotificationService notifications)
        {
            _events = eventStore;
            _notifications = notifications;
        }

        public async Task ImportEmailFilesAsync(IEnumerable<IFormFile> files)
        {
            var importedFiles = new List<IFormFile>();

            foreach (var f in files)
            {
                try
                {
                    using var ms = new MemoryStream();
                    using var msReader = new StreamReader(ms);
                    using var fs = f.OpenReadStream();
                    var mimeMsg = await MimeMessage.LoadAsync(fs);
                    await mimeMsg.WriteToAsync(ms);
                    ms.Position = 0;
                    var msg = await msReader.ReadToEndAsync();

                    await _events.Publish(new EmailReceivedEvent
                    {
                        MimeMessageId = mimeMsg.MessageId,
                        BodyMime = msg,
                    });

                    importedFiles.Add(f);
                }
                catch (Exception exc)
                {
                    await _notifications.PushErrorAsync($"Unable to import file {f?.Name}.", exc);
                }
            }

            if (importedFiles.Any())
            {
                await _notifications.PushSuccessAsync($"{importedFiles.Count()}/{files.Count()} files imported successfully.");
            }
        }
    }
}
