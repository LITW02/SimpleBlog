using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleBlog.Data;
using SimpleBlog.Models;

namespace SimpleBlog.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            if (page == null)
            {
                page = 1;
            }
            SimpleBlogManager manager = new SimpleBlogManager(Properties.Settings.Default.ConStr);
            IndexViewModel viewModel = new IndexViewModel();
            //IEnumerable<Post> posts = manager.GetPosts(page.Value);
            //List<PostWithCommentCount> postsWithCommentsCounts = new List<PostWithCommentCount>();
            //foreach (Post post in posts)
            //{
            //    PostWithCommentCount postWithCommentCount = new PostWithCommentCount();
            //    postWithCommentCount.Post = post;
            //    postWithCommentCount.CommentCount = manager.GetCommentCountForPost(post.Id);
            //    postsWithCommentsCounts.Add(postWithCommentCount);
            //}
            //viewModel.Posts = postsWithCommentsCounts;
            viewModel.Posts = manager.GetPosts(page.Value).Select(p => new PostWithCommentCount
            {
                Post = p,
                CommentCount = manager.GetCommentCountForPost(p.Id)
            });
            viewModel.CurrentPage = page.Value;
            viewModel.TotalPosts = manager.GetTotalPostCount();
            return View(viewModel);
        }

        public ActionResult Post(int postId)
        {
            SimpleBlogManager manager = new SimpleBlogManager(Properties.Settings.Default.ConStr);
            PostViewModel vieWModel = new PostViewModel();
            vieWModel.Post = manager.GetPostById(postId);
            vieWModel.Comments = manager.GetCommentsForPost(postId);
            return View(vieWModel);
        }

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Add(string title, string content)
        {
            SimpleBlogManager manager = new SimpleBlogManager(Properties.Settings.Default.ConStr);
            manager.AddPost(title, content);
            return Redirect("/home/index");
        }

        public ActionResult AddComment(string name, string content, int postId)
        {
            SimpleBlogManager manager = new SimpleBlogManager(Properties.Settings.Default.ConStr);
            manager.AddComment(postId, name, content);
            return Redirect("/home/post?postid=" + postId);
        }

    }
}
