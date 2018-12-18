# RESTful Request Handler
A helper for .NET platform that makes RESTful API calls easier than ever.

### Installation
* Available on NuGet: https://www.nuget.org/packages/RESTful.Handler/ [![NuGet](https://img.shields.io/badge/NUGET-1.0.0-green.svg)](https://www.nuget.org/packages/RESTful.Handler/)



### Basic Usage


#### Available Methods:
```MakeRequest``` : Doesn't return any Payload.

```MakeRequest<T>``` : An instance of the specified type is returned as Payload.

```MakeRequestForList<T>``` : A list of the specified type is returned as Payload.
  
  
#### Parameters:
```fullUri``` : Url of the web resource.

```method``` : Request method to use. The default is GET.

```parameters``` : A Dictionary<string, object> object, the key and value of which will be submitted as body parameters in case of POST and - PUT requests and appended to the url as query parameters in case of GET and DELETE.

```headers``` : Dictionary<string, object> object, the key and value of which will be sent as request headers.

```timeout``` : Request timeout in seconds. Default is 30.

```handler``` : An instance of a HttpClientHandler to use. If null a new instance of HttpClientHandler is created and used. The default value is null.

```files``` : List of FileParameter objects which will be sent as form-data. This parameter is used only in case of POST or PUT request. The properties of FileParameter object are :

    ParamName : The parameter by which the request for the file will be sent.  
    FileParameter : The actual name of the file.  
    File : Byte array of the file to send.
  
```formattedResponse``` : When set to true, the handler expects a json object with the given format as response from the server.
  ```
  {
    Success : true/false
    Message : "Message from server.",
    Title : "Title of message from server.",
    Payload : <actual object to be returned>
  }
  ```
  Properties that match are mapped in the response RequestHandler's response object. The default value of this parameter is false.
    
  
#### Response Format:
All the above mentioned available methods return instances of type ```RequestResult```, ```RequestResult<T>```, ```RequestListResult<T>``` respectively.

Following are the exposed properties :

```Success``` : Success boolean property of the formatted JSON object returnd by the server when FormattedResponse parameter is enabled.

```Message``` : Message string property of the formatted JSON object returnd by the server when FormattedResponse parameter is enabled.

```Title``` : Title string property of the formatted JSON object returnd by the server when FormattedResponse parameter is enabled.

```Payload``` : Payload property of the formatted JSON object returnd by the server when FormattedResponse is enabled.




### Examples

Returns a list of specified type in Payload
```
var listResp = await RESTful.RequestHandler.MakeRequestForList<Post>("https://jsonplaceholder.typicode.com/posts");
List<Post> posts = listResp.Payload; 
```
  
Returns specified reference type object in Payload
```
var objResp = await RESTful.RequestHandler.MakeRequest<User>("https://jsonplaceholder.typicode.com/posts/1");
```

Returns specified primitive type value in Payload
```
var intResp = await RESTful.RequestHandler.MakeRequest<int>("http://localhost:55779/api/values/GetInt");
```
                
Make a POST request with TestBody as additional URL query parameter; setting Authentication and Test as headers; and including a file with a request timeout of 30 seconds.
```
var binTest = await RESTful.RequestHandler.MakeRequest(
    "http://requestbin.fullcontact.com/1oqahqk1?urlParam=test&2ndUrlParam=test2", RESTful.RequestMethod.POST,
    new Dictionary<string, object>()
    {
             {"TestBody", DateTime.Now }
    },
    new Dictionary<string, string>()
    {
        {"Authentication", "auth header" },
        {"Test", "test header" }
    },
    30,
    files: new List<RESTful.FileParameter>()
    {
        new RESTful.FileParameter()
        {
            File = File.ReadAllBytes(@"C:\SpotlightImages\7809011bb4cc0a360a9146be98a87cf0b3fe561452b8456467782dc8074f684e.jpg"),
            FileName = "Spotlightimage.jpg",
            ParamName = "SpotlightImg"
        }
    }
);
```
