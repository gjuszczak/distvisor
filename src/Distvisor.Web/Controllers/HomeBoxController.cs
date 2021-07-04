using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/sec/home-box")]
    public class HomeBoxController : ControllerBase
    {
        private readonly IHomeBoxService _homeBoxService;

        public HomeBoxController(IHomeBoxService homeBoxService)
        {
            _homeBoxService = homeBoxService;
        }

        [HttpGet("devices")]
        public async Task<DeviceDto[]> GetDevices()
        {
            return await _homeBoxService.GetDevicesAsync();
        }

        [HttpPost("devices/{identifier}/turnOn")]
        public async Task TurnOnDevice(string identifier)
        {
            await _homeBoxService.SetDeviceParamsAsync(identifier, new { @switch = "on" });
        }

        [HttpPost("devices/{identifier}/turnOff")]
        public async Task TurnOffDevice(string identifier)
        {
            await _homeBoxService.SetDeviceParamsAsync(identifier, new { @switch = "off" });
        }

        [HttpGet("triggers/list")]
        public async Task<HomeBoxTriggerDto[]> ListTriggers()
        {
            return await _homeBoxService.ListTriggersAsync();
        }

        [HttpPost("triggers/add")]
        public async Task AddTrigger(AddHomeBoxTriggerDto dto)
        {
            await _homeBoxService.AddTriggerAsync(dto);
        }

        [HttpDelete("triggers/{id}/delete")]
        public async Task DeleteTrigger(Guid id)
        {
            await _homeBoxService.DeleteTriggerAsync(id);
        }

        [HttpPost("triggers/{id}/execute")]
        public async Task ExecuteTrigger(Guid id)
        {
            await _homeBoxService.ExecuteTriggerAsync(id);
        }
    }
}
