using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Models;

namespace Web.Api.Core.Dto
{
    public class LoginResponse
    {
        public bool Success { get; private set; }

        public AppUser AppUser { get; private set; }

        public Error Error { get; private set; }

        public string Token { get; set; }

        public LoginResponse(Error error, bool success)
        {
            AppUser = null;
            Error = error;
            Success = success;
        }

        public LoginResponse(AppUser appUser, bool success)
        {
            AppUser = appUser;
            Error = null;
            Success = success;
        }
    }
}
