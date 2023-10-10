using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibarary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Configuration;
using UdemyAuthServer.Core.Dtos;
using UdemyAuthServer.Core.Models;
using UdemyAuthServer.Core.Repositories;
using UdemyAuthServer.Core.Services;
using UdemyAuthServer.Core.UnitOfWork;

namespace UdemyAuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _client;  
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshToken;

        public AuthenticationService(IOptions<List<Client>> optionsClient, 
            ITokenService tokenService, 
            UserManager<UserApp> userManager,
            IUnitOfWork unitOfWork, 
            IGenericRepository<UserRefreshToken> userRefreshToken)
        {
            _client = optionsClient.Value;
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshToken = userRefreshToken;
        }
        public IGenericRepository<UserRefreshToken> UserRefreshToken => _userRefreshToken;

        public async Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto logInDto)
        {
            if (logInDto == null) throw new ArgumentNullException(nameof(logInDto));
            var user = await _userManager.FindByEmailAsync(logInDto.Email);
            if (user == null) return ResponseDto<TokenDto>.Fail("Email or Password is wrong!", 400, true);
            if (!await _userManager.CheckPasswordAsync(user, logInDto.Password))
            {
                return ResponseDto<TokenDto>.Fail("Email or Password is wrong!", 400, true);
            }

            var token = _tokenService.CreateToken(user);

            var userRefreshToken = await _userRefreshToken.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

            if (userRefreshToken == null)
            {
                await _userRefreshToken.AddAsync(new UserRefreshToken { UserId = user.Id, Code = token.RefreshToken, 
                Expiration = token.RefreshTokenExpiration});
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration; 
            }

            await _unitOfWork.CommitAsync();
            return ResponseDto<TokenDto>.Success(token, 200);
        }

        public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLogInDto clientLogInDto)
        {
            var client = _client.SingleOrDefault(x => x.Id == clientLogInDto.ClientId &&
            x.Secret == clientLogInDto.ClientSecret);
            if (client == null)
            {
                return ResponseDto<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);
            }
            var token = _tokenService.CreateTokenByClient(client);

            return ResponseDto<ClientTokenDto>.Success(token, 200);
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
        {
            var existRefresToken = await _userRefreshToken.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefresToken == null)
            {
                return ResponseDto<TokenDto>.Fail("Refresh token not found", 404, true);
            }

            var user = await _userManager.FindByIdAsync(existRefresToken.UserId);

            if (user == null)
            {
                return ResponseDto<TokenDto>.Fail("Refresh token not found", 404, true);

            }

            var tokenDto = _tokenService.CreateToken(user);

            existRefresToken.Code = tokenDto.RefreshToken;
            existRefresToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return ResponseDto<TokenDto>.Success(tokenDto, 200);
        }

        public async Task<ResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken)
        {
            var existRefresToken = await _userRefreshToken.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
            if (existRefresToken == null)
            {
                return ResponseDto<NoDataDto>.Fail("Refresh token not found", 404, true);
            }

            _userRefreshToken.Remove(existRefresToken);
            
            await _unitOfWork.CommitAsync();   
            
            return ResponseDto<NoDataDto>.Success(200);
        }
    }
}
