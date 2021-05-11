using System;
using System.ComponentModel.DataAnnotations;

namespace DependenciesMockingDemoProject.Web.Services.OpenWeatherClient
{
    public class OpenWeatherClientOptions
    {
        [Required] public Uri BaseAddress { get; set; }

        [Required] public string ApiKey { get; set; }
    }
}