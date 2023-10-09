using SharedLibarary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.Core.Services
{
    public interface IUserService
    {
        Task<ResponseDto<UserAppDto>> CreateUserAync(CreateUserDto createUserDto);
        Task<ResponseDto<UserAppDto>> GetUserByNameAsync(string userName);
    }
}
