using NUnit.Framework;

namespace DependenciesMockingDemoProject.Int
{
    public class IntegrationTest
    {
        [SetUp]
        public void SetUpLogger()
        {
            IntegrationLogger.Output = TestContext.Out;
            Integration.WeatherApi.Reset();
        }
    }
}