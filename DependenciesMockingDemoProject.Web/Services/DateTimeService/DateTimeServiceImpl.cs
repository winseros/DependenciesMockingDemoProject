using System;

namespace DependenciesMockingDemoProject.Web.Services.DateTimeService
{
    public class DateTimeServiceImpl : IDateTimeService
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}