using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTful
{
    public static class ResponseHelper
    {
        public static object CreateFormattedResponse(bool success, string title, string message, object payload = null)
        {
            return new { Success = success, Title = title, Message = message, Payload = payload };
        }
    }
}
