using ThreadShare.Models;
using ThreadShare.Data;
using ThreadShare.Service.Interfaces;
using ThreadShare.Repository.Interfaces;

namespace ThreadShare.Service.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task CreatePost(Post post)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                // Set other properties as needed
            };

            await _postRepository.AddAsync(post);
        }

        public async Task UpdatePost(Post post)
        {

        }

        public async Task DeletePost(int postId)
        {

        }

        public async Task<Post> GetPostById(int postId)
        {


        }

        public async Task<List<Post>> GetAllPosts()
        {

        }
    }
}
