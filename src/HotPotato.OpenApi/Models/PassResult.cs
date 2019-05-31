﻿
namespace HotPotato.OpenApi.Models
{
    public class PassResult : Result
    {
        public PassResult(string path, string method, int statusCode)
        {
            Path = path;
            Method = method;
            StatusCode = statusCode;
            State = State.Pass;
        }
    }
}
