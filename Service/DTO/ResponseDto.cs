using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO;

public class ResponseDto<T>
{
    public bool IsSuccess { get; set; } = true;
    public int StatusCode { get; set; } = 200;
    public string Message { get; set; } = "Success";
    public T? Body { get; set; }

    public ResponseDto(
        bool IsSuccess = true, 
        int StatusCode = 200, 
        string Message = "Success", 
        T? Body = default)
    {
        this.IsSuccess = IsSuccess;
        this.StatusCode = StatusCode;
        this.Message = Message;
        this.Body = Body;
    }
}

public class ResponseDto : ResponseDto<string>
{
    public ResponseDto(
        bool isSuccess = true,
        int statusCode = 200,
        string message = "Success"
    ) : base(isSuccess, statusCode, message)
    {
    }
}