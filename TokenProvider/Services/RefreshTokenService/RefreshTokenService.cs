using ArcelikAuthProvider.Context;
using ArcelikAuthProvider.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcelikAuthProvider.Services.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public RefreshTokenService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task AddAsync(string refreshToken, string userId)
        {
            await _applicationDbContext.RefreshTokens.AddAsync(new RefreshToken()
            {
                UserId = userId,
                Token = refreshToken

            });

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshTokenByUserId(string userId)
        {
            return await _applicationDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task UpdateAsync(RefreshToken refreshToken)
        {
            _applicationDbContext.RefreshTokens.Update(refreshToken);

            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetRefreshToken(string userId, string refreshToken)
        {
            var result = await _applicationDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken && x.UserId == userId);

            return result;
        }
    }
}
