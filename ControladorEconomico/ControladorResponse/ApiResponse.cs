using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;

namespace ControladorResponse
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? Errors { get; set; }

        public ApiResponse()
        {
            Errors = new List<string>();
        }
        public ApiResponse(T data, string? message = null)
        {
            Success = true;
            Message = message;
            Data= data;
            Errors=new List<string>();
        }
        public ApiResponse(string message, List<string>? errors = null)
        {
            Success=false;
            Message = message; 
            Errors=errors ?? new List<string>();
        }
    }
}
