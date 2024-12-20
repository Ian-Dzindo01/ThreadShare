﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThreadShare.Models;

namespace ThreadShare.Service.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        public RefreshToken GenerateRefreshToken();
        public string GetJwt();
        public bool TokenIsExpired(string token);
        public Task<string> RefreshJwt();
        public void SetJwt(string jwt);
        public Task SetRefreshToken(RefreshToken newRefreshToken, string email);
    }
}
