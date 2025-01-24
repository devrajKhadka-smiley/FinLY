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
        //private field
        private Users authenticatedUser;

        //encapsulated from outside of the outside and is controlled by the other method like getauthenticateduser and setauthenticateduser
        //get authenticated user method
        public Users GetAuthenticatedUser()
        {
            return authenticatedUser;
        }

        //set authenticated user method
        public void SetAuthenticatedUser(Users user)
        {
            authenticatedUser = user;
        }

        //this method check if user is authenticated
        public bool IsAuthenticated()
        {
            if (authenticatedUser != null)
            {
                return true;
            }

            return false;
        }
        // logout method
        public void LogOut()
        {
            authenticatedUser = null;
        }

        //get user id method
        public Guid GetUserId()
        {
            if (authenticatedUser == null)
            {
                return Guid.Empty; 
            }
            return authenticatedUser.UserId;
        }
    }
}
