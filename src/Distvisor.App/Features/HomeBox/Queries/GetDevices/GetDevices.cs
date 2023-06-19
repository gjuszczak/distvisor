using Distvisor.App.Core.Queries;
using Distvisor.App.Features.Common.Interfaces;
using Distvisor.App.Features.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Distvisor.App.Features.HomeBox.Queries.GetDevices
{
    public class GetDevices : PaginatedQuery, IQuery<DevicesListDto>
    {
    }

    public class GetDevicesHandler : IQueryHandler<GetDevices, DevicesListDto>
    {
        private readonly IAppDbContext _context;

        public GetDevicesHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<DevicesListDto> Handle(GetDevices request, CancellationToken cancellationToken)
        {
            var devices = await _context.HomeboxDevices
                .Select(entity => DeviceDto.FromEntity(entity))
                .ToPaginatedListAsync(request.First, request.Rows, cancellationToken);

            return new DevicesListDto(devices.Items, devices.TotalRecords, devices.First, devices.Rows);
        }
    }
}
