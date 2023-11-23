using Microsoft.AspNetCore.Mvc.Testing;

namespace SpendManagement.Domain.Integration.Tests.Fixtures
{
    public class SharedFixture
    {
        public SharedFixture()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Tests");
            var webAppFactory = new WebApplicationFactory<Program>();
            webAppFactory.CreateDefaultClient();
        }
    }
}
