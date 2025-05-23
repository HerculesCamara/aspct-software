using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPCTS.Models;

namespace ASPCTS.Services
{
    public interface IJwtService
    {
        Task<string> GenerateToken(Usuario usuario);
    }
}