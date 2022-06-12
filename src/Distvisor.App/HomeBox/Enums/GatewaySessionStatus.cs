using Distvisor.App.Core.Enums;

namespace Distvisor.App.HomeBox.Enums
{
    public class GatewaySessionStatus : Enumeration
    {
        public static readonly GatewaySessionStatus Open = new(1, nameof(Open));
        public static readonly GatewaySessionStatus Refreshing = new(2, nameof(Refreshing));
        public static readonly GatewaySessionStatus Closed = new(3, nameof(Closed));
        public static readonly GatewaySessionStatus Deleted = new(4, nameof(Deleted));

        private GatewaySessionStatus(int id, string name) : base(id, name) { }
    }
}
