﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models.Account
{
    public class RegisterResponseViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public RegisterResponseViewModel(IdentityUser user)
        {
            Id = user.Id;
            Email = user.Email;
        }
    }
}
