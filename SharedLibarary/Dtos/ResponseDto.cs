using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SharedLibarary.Dtos
{
    public class ResponseDto<T> where T : class
    {
        public T Data { get; private set; }
        public int Status { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public ErrorDto Error { get; private set; }

        public static ResponseDto<T> Success(T data, int status)
        {
            return new ResponseDto<T> { Data= data, Status = status, IsSuccessful = true };
        }

        public static ResponseDto<T> Success(int status)
        {
            return new ResponseDto<T> { Data = default, Status = status, IsSuccessful = true };
        }

        public static ResponseDto<T> Fail(ErrorDto errorDto, int status)
        {
            return new ResponseDto<T>
            {
                Error = errorDto,
                Status = status,
                IsSuccessful = false
            };
        }

        public static ResponseDto<T> Fail(string errorMessage,int status, bool isShow) 
        {
            var errorDto = new ErrorDto(errorMessage, isShow);
            return new ResponseDto<T> { Error = errorDto, Status = status, IsSuccessful = false };
        }
    }
}