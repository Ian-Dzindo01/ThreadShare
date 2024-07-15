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
    }
}
