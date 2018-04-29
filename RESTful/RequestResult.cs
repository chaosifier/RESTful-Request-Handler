using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace RESTful
{
    public class RequestResult
    {
        public HttpStatusCode Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool? Success { get; set; }
    }

    public class CombinedRequestResult<T> : RequestResult
    {
        public T SinglePayload { get; set; }
        public List<T> ListPayload { get; set; }
    }

    public class RequestResult<T> : RequestResult
    {
        public T Payload { get; set; }
    }

    public class RequestValueResult<T> : RequestResult where T : struct
    {
        public T Payload { get; set; }
    }

    public class RequestObjectResult<T> : RequestResult where T : class
    {
        public T Payload { get; set; }
    }

    public class RequestListResult<T> : RequestResult
    {
        public List<T> Payload { get; set; }
    }
}
