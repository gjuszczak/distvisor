﻿using System;

namespace Distvisor.App.Features.HomeBox.Services.Gateway
{
    public class GatewayActiveSession
    {
        public Guid SessionId { get; set; }
        public string AccessToken { get; set; }
    }
}
