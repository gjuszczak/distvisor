using Distvisor.Web.Data.EventSourcing.Core;

namespace Distvisor.Web.Data.EventSourcing
{
    public class AddUserEvent
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class AddUserEvent_v1
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class AddUserEventHandler : IEventHandler<AddUserEvent>, IEventHandler<AddUserEvent_v1>
    {
        public void Handle(AddUserEvent payload)
        {
            
        }

        public void Handle(AddUserEvent_v1 payload)
        {
           
        }
    }
}
