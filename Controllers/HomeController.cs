using Blog.Data;
using Blog.Data.FileManager;
using Blog.Data.Repository;
using Blog.Models;
using Blog.Models.Comments;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
            //var comment = new MainComment();
        }

        public IActionResult Index()
        {
            var posts = _repo.GetAllPost();
            return View(posts);
        }

        
        [Authorize]
        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);

            return View(post);
        }

        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var extension = image.Substring(image.LastIndexOf('.') + 1);

            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{extension}");
                
        }

        [Authorize]
        [HttpGet]        
        public IActionResult Edit(int? id)
        {
            //check if the id is nullable or not
            if (id == null)
            {
                return View(new Post());
            }
            else
            {                
                var post = _repo.GetPost((int)id);
                return View(post);

            }
            //give it a model to avoid any errors
        }

        [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Post", new { id = vm.PostId });
            }

            var post = _repo.GetPost(vm.PostId);

            //if the id is bigger than 0 it is a subcomment, otherwise it is the main comment

            //if (vm.Comment.Id > 0)
            //{
            //    var comment = _repo.GetComment(vm.Comment.Id);
            //    comment.Body = vm.Comment.Body;
            //    _repo.UpdateComment(comment);
            //}
            //else
            //{
            //    var comment = new SubComment
            //    {
            //        PostId = vm.Comment.PostId,
            //        Body = vm.Comment.Body,
            //        Created = DateTime.Now,
            //        UserName = User.Identity.Name
            //    };
            //    _repo.AddSubComment(comment);
            //}


            if (vm.MainCommentId == 0 )
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,
                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };

                _repo.AddSubComment(comment);
            }

            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = vm.PostId });
        }       
        
    }
}
