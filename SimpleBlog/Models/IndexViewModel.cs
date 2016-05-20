using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleBlog.Data;

namespace SimpleBlog.Models
{
    public class IndexViewModel
    {
        public IEnumerable<PostWithCommentCount> Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPosts { get; set; }

        public bool ShouldDisplayOlderButton()
        {
            return CurrentPage * 3 < TotalPosts;
        }
    }

    public class PostWithCommentCount
    {
        public Post Post { get; set; }
        public int CommentCount { get; set; }
    }
}