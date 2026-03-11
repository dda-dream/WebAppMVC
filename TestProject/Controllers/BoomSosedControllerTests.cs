using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using WebAppMVC;

namespace TestProject.Controllers
{
    public class BoomSosedControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BoomSosedControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        /*
        [Fact]
        public async Task Enabled_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/BoomSosed/Enabled");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        */

        [Fact]
        public async Task Enabled_Should_BeGreaterThan_50()
        {
            var response = await _client.GetAsync("/BoomSosed/Enabled");
            var body = await response.Content.ReadAsStringAsync();
            var value = int.Parse(body);
            value.Should().BeGreaterThan(50);
        }

        [Fact]
        public async Task Enabled_Should_BeLessThan_0()
        {
            var response = await _client.GetAsync("/BoomSosed/Enabled");
            var body = await response.Content.ReadAsStringAsync();
            var value = int.Parse(body);
            value.Should().BeLessThan(50);
        }




    }
}
