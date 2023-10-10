using SharedLibarary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto logInDto);
        Task<ResponseDto<TokenDto>> CreateTokenByRefreshToken(string refreshToken); 
        Task<ResponseDto<NoDataDto>> RevokeRefreshToken(string refreshToken);
        ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLogInDto clientLogInDto);
    }
}
