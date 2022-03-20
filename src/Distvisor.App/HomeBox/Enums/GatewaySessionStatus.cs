using Distvisor.App.Core.Enums;

namespace Distvisor.App.HomeBox.Enums
{
    public class GatewaySessionStatus : Enumeration
    {
        public static GatewaySessionStatus Open = new(1, nameof(Open));
        public static GatewaySessionStatus Refreshing = new(2, nameof(Refreshing));
        public static GatewaySessionStatus Closed = new(3, nameof(Closed));

        private GatewaySessionStatus(int id, string name) : base(id, name) { }
    }
}
