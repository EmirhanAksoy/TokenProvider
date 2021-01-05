using ArcelikAuthProvider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Services.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        public Task<RefreshToken> GetRefreshTokenByUserId(string userId);

        public Task AddAsync(string refreshToken, string userId);

        public Task UpdateAsync(RefreshToken refreshToken);

        public Task<RefreshToken> GetRefreshToken(string userId, string refreshToken);
    }
}
