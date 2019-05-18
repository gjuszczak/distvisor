using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Distvisor.Services
{
    public class AuthService
    {
        private bool isAuthenticated = true;

        public bool IsAuthenticated()
        {
            return isAuthenticated;
        }

        public void Login(string login, string password)
        {
            if (login == "admin" && password == "admin")
                isAuthenticated = true;
            else
                isAuthenticated = false;
        }
    }
}
