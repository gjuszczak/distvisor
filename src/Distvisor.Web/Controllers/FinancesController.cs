using Distvisor.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FinancesController : ControllerBase
    {
        private readonly IMailgunClient _mailgun;

        public FinancesController(IMailgunClient mailgun)
        {
            _mailgun = mailgun;
        }

        [HttpGet]
        public async Task<object> GetEmail()
        {
            return await _mailgun.ListStoredEmailsAsync();
        }
    }
}
