using System;
using System.Collections.Generic;
using System.Net;

namespace TrabalhoAPI
{
    public class ResultData<T>
    {
        public List<T> Data { get; set; } = new List<T>();
        public bool Error { get; set; }
        public List<string> Message { get; set; } = new List<string>();
        public HttpStatusCode Status { get; set; }
    }
}
