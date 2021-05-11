using System.ComponentModel.DataAnnotations;

namespace DependenciesMockingDemoProject.Web.DataLayer
{
    public class DataLayerOptions
    {
        [Required]
        public string ConnectionString { get; set; }
    }
}