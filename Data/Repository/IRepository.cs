using Blog.Models;
using Blog.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Repository
{
    public interface IRepository
    {
        Post GetPost(int id);
        List<Post> GetAllPost();
        void RemovePost(int id);
        void UpdatePost(Post post);
        void AddPost(Post post);

        //void GetComment(int id);
        
        void AddSubComment(SubComment subComment);
        Task<bool> SaveChangesAsync();



    }
}
