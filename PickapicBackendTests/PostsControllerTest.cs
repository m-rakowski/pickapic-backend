using Microsoft.EntityFrameworkCore;
using PickapicBackend.Controllers;
using PickapicBackend.Data;
using PickapicBackend.Model;
using Xunit;

namespace PicapicBackendTests
{
    public class PostsControllerTest
    {
        [Fact]
        public async void ShouldReturnEmptyArrayOfPosts()
        {
            var options = new DbContextOptionsBuilder<DataContext>().UseInMemoryDatabase(databaseName: "Abc").Options;
            var context = new DataContext(options);
            var controller = new PostsController(null, context);

            var result = await controller.GetPosts();

            Assert.Empty(result.Value);
        }

        private void Seed(DataContext context)
        {
            var posts = new[] {
                new Post {
                    PostId = 0,
                    Question = "This or.. that?"
                }
            };
            context.Posts.AddRange(posts);
            context.SaveChanges();
        }
    }
}
