using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ThreadShare.Models;

namespace ThreadShare.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
