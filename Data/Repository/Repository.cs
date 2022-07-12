using Blog.Models;
using Blog.Models.Comments;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public class Repository : IRepository
    {
        private ApplicationDbContext _ctx;

        public Repository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }
                              
        public Post GetPost(int id)
        {
            //take a single post in a whole table, check its id, if its equal to the id, the first one that is equal, return it
            //return _ctx.Posts.FirstOrDefault(p => p.Id == id);
            return _ctx.Posts
                .Include(p => p.MainComments)
                    .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.Id == id);           

        }

        public void AddSubComment(SubComment subComment)
        {
            _ctx.SubComments.Add(subComment);
        }

        public List<Post> GetAllPost()
        {
            return _ctx.Posts.ToList();
        }

        //public List<Post> GetAllPosts(string category)
        //{
        //    //return _ctx.Posts.Where(p => p.Category == category).ToList();

        //    Func<Post, bool> InCategory = (post) => { return post.Category.ToLower().Equals(category.ToLower()); };

        //    return _ctx.Posts
        //        .Where(post => InCategory(post))
        //        .ToList();
        //}

        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }

        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
        }

        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }


    }
}
