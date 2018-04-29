using System;
using System.Collections.Generic;
using System.Text;

namespace RESTful
{
    public enum RequestMethod
    {
        /// <summary>
        /// The GET method requests a representation of the specified resource.Requests using GET should only retrieve data.
        /// </summary>
        GET,
        /// <summary>
        /// The POST method is used to submit an entity to the specified resource, often causing a change in state or side effects on the server.
        /// </summary>
        POST,
        /// <summary>
        /// The PUT method replaces all current representations of the target resource with the request payload.
        /// </summary>
        PUT,
        /// <summary>
        /// The DELETE method deletes the specified resource.
        /// </summary>
        DELETE
    }
}