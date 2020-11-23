using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using PactNet;

namespace TodoApi.Test.TodoWeb
{
    [TestFixture]
    public class TodoWebContractTests
    {
        private IHost _host;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(
                    webBuilder =>
                    {
                        webBuilder.UseStartup<ContractTestStartup>();
                        webBuilder.UseSetting(
                            WebHostDefaults.ApplicationKey,
                            typeof(Startup).Assembly.GetName().Name);
                    })
                .Build();
            _host.Start();
        }

        [Test]
        public void PactVerify()
        {
            const string providerBaseUri = "http://localhost:5000";
            IPactVerifier pactVerifier = new PactVerifier(new PactVerifierConfig());
            pactVerifier
                .ProviderState($"{providerBaseUri}{Constants.ProviderStateEndpoint}")
                .ServiceProvider("TodoApi", providerBaseUri)
                .HonoursPactWith("todo-web")
                .PactUri("/Users/psilpsakulsu/workspace/todo-web/pact/pacts/todo-web-todoapi.json")
                .Verify();
        }
        
        [OneTimeTearDown]
        public void TearDown()
        {
            _host.Dispose();
        }
    }
}