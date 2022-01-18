using Distvisor.App.Common;
using Distvisor.App.Core.Queries;
using Distvisor.App.HomeBox.Services.Gateway;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Queries.GetDevices
{
    public class GetDevices : IQuery<DeviceDto[]>
    {
    }

    public class GetDevicesHandler : IQueryHandler<GetDevices, DeviceDto[]>
    {
        private readonly IAppDbContext _context;
        private readonly IGatewayClient _gateway;

        public GetDevicesHandler(IAppDbContext context, IGatewayClient gateway)
        {
            _context = context;
            _gateway = gateway;
        }

        public async Task<DeviceDto[]> Handle(GetDevices request, CancellationToken cancellationToken)
        {
            var gatewayDevices = await _gateway.GetDevicesAsync();
            var storedDevices = await _context.HomeboxDevices.ToListAsync();
            return new DeviceDto[0];
        }
    }
}
