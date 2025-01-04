using FinLY.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace FinLY.Services
{
    public class AuthenticationStateService
    {
        private Users authenticatedUser;

        public Users GetAuthenticatedUser()
        {
            return authenticatedUser;
        }

        public void SetAuthenticatedUser(Users user)
        {
            authenticatedUser = user;
        }

        public bool IsAuthenticated()
        {
            if (authenticatedUser != null)
            {
                return true;
            }

            return false;
        }

        public void LogOut()
        {
            authenticatedUser = null;
        }

        public Guid GetUserId()
        {
            if (authenticatedUser == null)
            {
                // User is not authenticated
                return Guid.Empty; // You can return an error or prompt to log in
            }
            return authenticatedUser.UserId;
        }
    }
}
