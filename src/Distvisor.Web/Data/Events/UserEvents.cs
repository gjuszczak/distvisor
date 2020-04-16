﻿using Distvisor.Web.Data.Entities;
using Distvisor.Web.Data.Events.Core;
using Distvisor.Web.Data.Reads;
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
        private readonly ReadStore _context;

        public AddUserEventHandler(ReadStore context)
        {
            _context = context;
        }

        public void Handle(AddUserEvent payload)
        {
            _context.Users.Insert(new UserEntity
            {
                Id = payload.Id,
                LockoutUtc = DateTime.MinValue.ToUniversalTime(),
                Username = payload.Username,
                PasswordHash = payload.PasswordHash
            });
        }
    }
}
