﻿namespace Moongazing.Empyrean.WebApi;

public class WebApiConfiguration
{
    public string ApiDomain { get; set; }
    public string[] AllowedOrigins { get; set; }

    public WebApiConfiguration()
    {
        ApiDomain = string.Empty;
        AllowedOrigins = [];
    }

    public WebApiConfiguration(string apiDomain, string[] allowedOrigins)
    {
        ApiDomain = apiDomain;
        AllowedOrigins = allowedOrigins;
    }
}