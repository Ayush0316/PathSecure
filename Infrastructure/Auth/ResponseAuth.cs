using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth
{
    public class ResponseAuth
    {
        public bool IsSuccess { get; set; } = false;
        public int StatusCode { get; set; } = 484;
        public string Message { get; set; }

        public ResponseAuth(string msg)
        {
            Message = msg;
        }
    }
}
