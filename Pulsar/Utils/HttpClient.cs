﻿namespace Pulsar.Utils;

public sealed class HttpClient
{
    private HttpClient()
    { }

    private static readonly Lazy<System.Net.Http.HttpClient> lazy = new Lazy<System.Net.Http.HttpClient>(() => new System.Net.Http.HttpClient());

    public static System.Net.Http.HttpClient Client => lazy.Value;

    public static string GetString(string url)
    {
        return lazy.Value.GetStringAsync(url).Result;
    }

    public static HttpResponseMessage SendRequest(HttpRequestMessage request)
    {
        return lazy.Value.SendAsync(request).Result;
    }

    public static Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
    {
        return lazy.Value.SendAsync(request);
    }
}