using System;
using System.Net;

namespace DependenciesMockingDemoProject.Web.Services.OpenWeatherClient
{
    public class HttpStatusException: ApplicationException
    {
        public HttpStatusException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}