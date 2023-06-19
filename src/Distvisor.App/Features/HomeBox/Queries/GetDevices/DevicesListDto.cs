using Distvisor.App.Features.Common.Models;
using System.Collections.Generic;

namespace Distvisor.App.Features.HomeBox.Queries.GetDevices
{
    public class DevicesListDto : PaginatedList<DeviceDto>
    {
        public DevicesListDto(List<DeviceDto> items, int totalRecords, int first, int rows)
            : base(items, totalRecords, first, rows) { }
    }
}
