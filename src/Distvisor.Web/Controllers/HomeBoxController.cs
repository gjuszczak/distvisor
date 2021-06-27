using Distvisor.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Distvisor.Web.Controllers
{
    [ApiController]
    //[Authorize]
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
            await _homeBoxService.SetDeviceParamsAsync(identifier, "on");
        }

        [HttpPost("devices/{identifier}/turnOff")]
        public async Task TurnOffDevice(string identifier)
        {
            await _homeBoxService.SetDeviceParamsAsync(identifier, "off");
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

            /*
            {
              "enabled": true,
              "sources": [
                {
                  "type": "Rf433Receiver",
                  "matchParam": "1234567"
                }
              ],
              "targets": [
                {
                  "deviceIdentifier": "test"
                }
              ],
              "actions": [
                {
                  "lastExecutedActionNumber": null,
                  "lastExecutedActionMinDelayMs": null,
                  "lastExecutedActionMaxDelayMs": null,
                  "isDeviceOnline": false,
                  "isDeviceOn": false,
                  "payload": { "switch": "on" }
                }
              ]
            }
            */
        }

        [HttpDelete("triggers/{id}/delete")]
        public async Task DeleteTrigger(Guid id)
        {
            await _homeBoxService.DeleteTriggerAsync(id);
        }
    }
}
