using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RESTful
{
    public class RequestHandler
    {       
        public static async Task<RequestResult> MakeRequest(string fullUri, RequestMethod method = RequestMethod.GET, Dictionary<string, object> parameters = null, Dictionary<string, string> headers = null, int timeout = 30, HttpClientHandler handler = null, List<FileParameter> files = null, bool formattedResponse = false)
        {
            var result = await MakeCommonRequest<PayloadlessRequest>(fullUri, method, parameters, headers, timeout, handler, files, formattedResponse, false);
            return new RequestResult()
            {
                Success = result.Success,
                Title = result.Title,
                Message = result.Message,
                Status = result.Status
            };
        }

        public static async Task<RequestResult<T>> MakeRequest<T>(string fullUri, RequestMethod method = RequestMethod.GET, Dictionary<string, object> parameters = null, Dictionary<string, string> headers = null, int timeout = 30, HttpClientHandler handler = null, List<FileParameter> files = null, bool formattedResponse = false)
        {
            var result = await MakeCommonRequest<T>(fullUri, method, parameters, headers, timeout, handler, files, formattedResponse, false);
            return new RequestResult<T>()
            {
                Success = result.Success,
                Title = result.Title,
                Message = result.Message,
                Payload = result.SinglePayload,
                Status = result.Status
            };
        }

        public static async Task<RequestListResult<T>> MakeRequestForList<T>(string fullUri, RequestMethod method = RequestMethod.GET, Dictionary<string, object> parameters = null, Dictionary<string, string> headers = null, int timeout = 30, HttpClientHandler handler = null, List<FileParameter> files = null, bool formattedResponse = false)
        {
            var result = await MakeCommonRequest<T>(fullUri, method, parameters, headers, timeout, handler, files, formattedResponse, true);
            return new RequestListResult<T>()
            {
                Success = result.Success,
                Title = result.Title,
                Message = result.Message,
                Payload = result.ListPayload,
                Status = result.Status
            };
        }

        private static async Task<HttpResponseMessage> GetHttpResponseMessage(string fullUri, RequestMethod method = RequestMethod.GET, Dictionary<string, object> parameters = null, Dictionary<string, string> headers = null, int timeout = 5, HttpClientHandler handler = null, List<FileParameter> files = null, bool formattedResponse = false)
        {
            try
            {
                HttpResponseMessage responseMessage = null;

                using (var client = handler == null ? new HttpClient() : new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(timeout);
                    // comment ?
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                        }
                    }


                    if (method == RequestMethod.GET || method == RequestMethod.DELETE)
                    {
                        if (parameters != null)
                        {
                            var uriBuilder = new UriBuilder(fullUri);
                            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

                            foreach(var pair in parameters)
                            {
                                query[pair.Key] = pair.Value.ToString();
                            }
                            
                            uriBuilder.Query = query.ToString();
                            fullUri = uriBuilder.ToString();
                        }

                        responseMessage = method == RequestMethod.GET ? await client.GetAsync(fullUri) : await client.DeleteAsync(fullUri);
                    }
                    else if (method == RequestMethod.POST || method == RequestMethod.PUT)
                    {
                        var content = new MultipartFormDataContent();

                        if (parameters != null)
                        {
                            foreach (var pair in parameters)
                            {
                                StringContent jsonContent = new StringContent(
                                    JsonConvert.SerializeObject(pair.Value),
                                    new UTF8Encoding(),
                                    "application/json");
                                content.Add(jsonContent, pair.Key);
                            }
                        }

                        if (files != null)
                        {
                            foreach (var file in files)
                            {
                                ByteArrayContent fileContent = new ByteArrayContent(file.File);
                                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { FileName = file.FileName, Name = file.ParamName };
                                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                                content.Add(fileContent);
                            }
                        }

                        responseMessage = method == RequestMethod.POST ? await client.PostAsync(fullUri, content) : await client.PutAsync(fullUri, content);
                    }
                }
                return responseMessage;
            }
            catch(TaskCanceledException)
            {
                throw new Exception("Request timed out.");
            }
            catch (Exception ex)
            {
                throw new Exception("Request failed. " + ex.Message);
            }
        }

        private static async Task<CombinedRequestResult<T>> MakeCommonRequest<T>(string fullUri, RequestMethod method = RequestMethod.GET, Dictionary<string, object> parameters = null, Dictionary<string, string> headers = null, int timeout = 30, HttpClientHandler handler = null, List<FileParameter> files = null, bool formattedResponse = false, bool forList = false)
        {
            var responseMessage = await GetHttpResponseMessage(fullUri, method, parameters, headers, timeout, handler, files, formattedResponse);

            var requestResult = new CombinedRequestResult<T>()
            {
                Status = responseMessage.StatusCode
            };

            if (typeof(T) == typeof(PayloadlessRequest) && !formattedResponse)
            {
                return requestResult;
            }

            string returnString = await responseMessage.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(returnString))
            {
                JToken payloadJToken = null;
                if (formattedResponse)
                {
                    try
                    {
                        JObject formattedResponseJObj = JObject.Parse(returnString);
                        requestResult.Message = formattedResponseJObj.GetValue("Message", StringComparison.OrdinalIgnoreCase).ToString();
                        requestResult.Title = formattedResponseJObj.GetValue("Title", StringComparison.OrdinalIgnoreCase).ToString();
                        requestResult.Success = (bool)formattedResponseJObj.GetValue("Success", StringComparison.OrdinalIgnoreCase);

                        if (typeof(T) == typeof(PayloadlessRequest))
                        {
                            return requestResult;
                        }

                        payloadJToken = formattedResponseJObj.GetValue("Payload", StringComparison.OrdinalIgnoreCase);
                    }
                    catch (Exception)
                    {
                        throw new Exception($"Response is not in the right format. The response must be a JSON object with properties as below : " +
                            $"{Environment.NewLine}  string : Message " +
                            $"{Environment.NewLine}  string : Title " +
                            $"{Environment.NewLine}  boolean : Success " +
                            $"{Environment.NewLine}  any type : Payload"
                            );
                    }
                }
                else
                {
                    payloadJToken = JToken.Parse(returnString);
                }

                if (payloadJToken != null)
                {
                    if (forList)
                    {
                        requestResult.ListPayload = payloadJToken.ToObject<List<T>>();
                    }
                    else
                    {
                        if (typeof(T).IsValueType && default(T) != null)
                        {
                            requestResult.SinglePayload = payloadJToken.Value<T>();
                        }
                        else
                        {
                            requestResult.SinglePayload = payloadJToken.ToObject<T>();
                        }
                    }
                }
            }

            return requestResult;
        }

        private sealed class PayloadlessRequest { }
    }
}