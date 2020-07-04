using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads.Core;
using System;

namespace Distvisor.Web.Data.Events
{
    public class AddUserEvent
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }

    public class AddUserEventHandler : IEventHandler<AddUserEvent>
    {
        private readonly ReadStoreContext _context;

        public AddUserEventHandler(ReadStoreContext context)
        {
            _context = context;
        }

        public void Handle(AddUserEvent payload)
        {
            _context.Users.Add(new UserEntity
            {
                Id = payload.Id,
                LockoutUtc = DateTime.MinValue.ToUniversalTime(),
                Username = payload.Username,
                PasswordHash = payload.PasswordHash
            });
            _context.SaveChanges();
        }
    }
}
