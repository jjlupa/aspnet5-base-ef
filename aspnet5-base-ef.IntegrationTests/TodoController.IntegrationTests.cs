using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using aspnet5_base_ef.DTOs;
using Newtonsoft.Json;
using Xunit;

namespace aspnet5_base_ef.IntegrationTests
{
    // This is a reasonable place to talk about integration tests.  Generally speaking, integration tests should run against an
    // environment under test.  In the current situation, we are scaffolding this test with a fixture which initializes an in
    // memory application with an in memory database.  Database faking is mostly useless in this layer, since we have already
    // sorted that out in the unit tests of the repository.  The in-memory webhost is a bit of a value add, in that it's fully
    // executing all the middleware, but in terms of configuration, this is much more interesting when run againt some other
    // external infrastructure.
    //
    // The test as written doesn't solve that problem, more work TODO for later.
    //
    public class TodoItemControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _httpclient;

        public TodoItemControllerIntegrationTests(CustomWebApplicationFactory<Startup> factory)
        {
            _httpclient = factory.CreateClient();
        }

        [Fact]
        public async Task TodoItemController_CanGetTodoItems()
        {
            var httpResponse = await _httpclient.GetAsync("/api/TodoItems");
            httpResponse.EnsureSuccessStatusCode();

            var jsonstr = await httpResponse.Content.ReadAsStringAsync();

            var items = JsonConvert.DeserializeObject<IEnumerable<TodoItemDTOv1>>(jsonstr);
            Assert.Contains(items, i => i.Name == "Chicken in the henhouse pick'n at dough");
            Assert.Contains(items, i => i.Name == "Babe you hurt me, and you know I hurt you too");
        }
    }
}

