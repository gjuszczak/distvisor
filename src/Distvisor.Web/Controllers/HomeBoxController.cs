using Distvisor.Web.Data;
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

        [HttpPost("api-login")]
        public async Task ApiLogin(HomeBoxApiLoginDto dto)
        {
            await _homeBoxService.ApiLoginAsync(dto);
        }

        [HttpGet("devices")]
        public async Task<HomeBoxDeviceDto[]> GetDevices()
        {
            return await _homeBoxService.GetDevicesAsync();
        }

        [HttpPost("devices/{identifier}/updateDetails")]
        public async Task<HomeBoxDeviceDto> UpdateDeviceDetails(string identifier, UpdateHomeBoxDeviceDto dto)
        {
            dto.Id = identifier;
            return await _homeBoxService.UpdateDeviceDetailsAsync(dto);
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

        [HttpGet("triggers")]
        public async Task<HomeBoxTriggerDto[]> ListTriggers()
        {
            return await _homeBoxService.ListTriggersAsync();
        }

        [HttpPost("triggers")]
        public async Task CreateTrigger(HomeBoxTriggerDto dto)
        {
            await _homeBoxService.CreateTriggerAsync(dto);
        }

        [HttpPut("triggers/{id}")]
        public async Task UpdateTrigger(Guid id, HomeBoxTriggerDto dto)
        {
            dto.Id = id;
            await _homeBoxService.UpdateTriggerAsync(dto);
        }

        [HttpDelete("triggers/{id}")]
        public async Task DeleteTrigger(Guid id)
        {
            await _homeBoxService.DeleteTriggerAsync(id);
        }

        [HttpPost("triggers/{id}/execute")]
        public async Task ExecuteTrigger(Guid id)
        {
            await _homeBoxService.ExecuteTriggerAsync(id);
        }

        [HttpPost("triggers/{id}/toggle")]
        public async Task ToggleTrigger(Guid id, bool enable)
        {
            await _homeBoxService.ToggleTriggerAsync(id, enable);
        }
    }
}
