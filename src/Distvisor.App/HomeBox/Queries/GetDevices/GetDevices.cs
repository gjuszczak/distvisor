using Distvisor.App.Common;
using Distvisor.App.Core.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.HomeBox.Queries.GetDevices
{
    public class GetDevices : IQuery<IEnumerable<DeviceDto>>
    {
    }

    public class GetDevicesHandler : IQueryHandler<GetDevices, IEnumerable<DeviceDto>>
    {
        private readonly IAppDbContext _context;

        public GetDevicesHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DeviceDto>> Handle(GetDevices request, CancellationToken cancellationToken)
        {
            var devices = await _context.HomeboxDevices.ToListAsync(cancellationToken);
            var deviceDtos = devices.Select(DeviceDto.FromEntity).ToArray();
            return deviceDtos;
        }
    }
}
