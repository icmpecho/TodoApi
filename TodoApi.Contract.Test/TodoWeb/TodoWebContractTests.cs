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
                        webBuilder.UseStartup<Startup>();
                    })
                .Build();
            _host.Start();
        }

        [Test]
        public void PactVerify()
        {
            IPactVerifier pactVerifier = new PactVerifier(new PactVerifierConfig());
            pactVerifier
                .ServiceProvider("TodoApi", "http://localhost:5000")
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