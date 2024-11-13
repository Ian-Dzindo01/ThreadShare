using ThreadShare.Models;
using ThreadShare.Data;
using ThreadShare.Service.Interfaces;
using ThreadShare.Repository.Interfaces;
using ThreadShare.DTOs.Entites;
using ThreadShare.Repository.Implementations;

namespace ThreadShare.Service.Implementations
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task CreatePost(PostViewModel model)
        {
            var post = new Post
            {
                Title = model.Title,
                Body = model.Body,
                UserId = model.UserId,
                ForumId = model.ForumId
            };

            await _postRepository.Add(post);
        }

        public async Task UpdatePost(PostViewModel model, int postId)
        {
            Post existingPost = await _postRepository.GetById(postId);

            if (existingPost != null)
            {
                existingPost.Title = model.Title;
                existingPost.Body = model.Body;

                await _postRepository.Update(existingPost);
            }
            else
            {
                throw new ArgumentException("Post not found.");
            }
        }

        public async Task DeletePost(int postId)
        {
            await _postRepository.Delete(postId); 
        }

        public async Task<Post> GetPostById(int postId)
        {
            return await _postRepository.GetById(postId);
        }

        public async Task<IEnumerable<Post>> GetNewestPosts()
        {
            return await _postRepository.GetNewest();
        }
    }
}
