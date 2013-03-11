using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MobileOAuth
{
    public static class HttpWebRequestExtensions
    {
        public static Task<Stream> GetRequestStreamAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<Stream>();
            request.BeginGetRequestStream(asyncResult =>
                {
                    try
                    {
                        HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
                        Stream stream = (Stream)webRequest.EndGetRequestStream(asyncResult);
                        taskComplete.TrySetResult(stream);
                    }
                    catch (Exception ex)
                    {
                        taskComplete.SetException(ex);
                    }
                }, request);
            return taskComplete.Task;
        } 

        public static Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request)
        {
            var taskComplete = new TaskCompletionSource<HttpWebResponse>();
            request.BeginGetResponse(asyncResult =>
                {
                    try
                    {
                        HttpWebRequest webRequest = (HttpWebRequest)asyncResult.AsyncState;
                        HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult);
                        taskComplete.TrySetResult(webResponse);
                    }
                    catch (WebException ex)
                    {
                        HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                        taskComplete.TrySetResult(webResponse);
                    }
                    catch (Exception ex)
                    {
                        taskComplete.SetException(ex);
                    }
                }, request);
            return taskComplete.Task;
        }
    }
}
