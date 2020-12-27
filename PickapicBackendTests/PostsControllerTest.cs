using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using PickapicBackend.Model;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class PostsControllerTests : IClassFixture<WebApplicationFactory<PickapicBackend.Startup>>
{
    HttpClient _client { get; }

    public PostsControllerTests(WebApplicationFactory<PickapicBackend.Startup> fixture)
    {
        _client = fixture.CreateClient();
    }

    [Fact]
    public async Task ShouldReturnEmptyArray()
    {
        var response = await _client.GetAsync("/api/posts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var posts = JsonConvert.DeserializeObject<Post[]>(await response.Content.ReadAsStringAsync());

        Assert.Empty(posts);
    }
}