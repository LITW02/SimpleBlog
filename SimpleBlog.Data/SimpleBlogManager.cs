using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SimpleBlog.Data
{
    public class SimpleBlogManager
    {
        private string _connectionString;

        public SimpleBlogManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Post> GetPosts(int page)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                int offset = (page - 1) * 3;
                command.CommandText = "SELECT * FROM Posts ORDER BY Date DESC OFFSET @offset ROWS FETCH NEXT 3 ROWS ONLY";
                command.Parameters.AddWithValue("@offset", offset);
                List<Post> posts = new List<Post>();
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    posts.Add(GetPostFromReader(reader));
                }

                return posts;
            }
        }

        public Post GetPostById(int postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Posts WHERE Id = @id";
                command.Parameters.AddWithValue("@id", postId);
                connection.Open();
                var reader = command.ExecuteReader();
                reader.Read();
                return GetPostFromReader(reader);
            }
        }

        public IEnumerable<Comment> GetCommentsForPost(int postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM Comments WHERE PostId = @id";
                command.Parameters.AddWithValue("@id", postId);
                connection.Open();
                var reader = command.ExecuteReader();
                List<Comment> comments = new List<Comment>();
                while (reader.Read())
                {
                    Comment comment = new Comment();
                    comment.Id = (int)reader["Id"];
                    comment.Name = (string)reader["Name"];
                    comment.Content = (string)reader["Content"];
                    comment.Date = (DateTime)reader["Date"];
                    comment.PostId = (int)reader["PostId"];
                    comments.Add(comment);
                }

                return comments;
            }
        }

        public void AddPost(string title, string content)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Posts (Title, Content, Date) VALUES " +
                                      "(@title, @content, @date)";
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@content", content);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void AddComment(int postId, string name, string content)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO Comments (Name, Content, Date, PostId) VALUES " +
                                      "(@name, @content, @date, @postId)";
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@content", content);
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@postId", postId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetTotalPostCount()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Posts";
                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        public int GetCommentCountForPost(int postId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM Comments WHERE PostId = @postId";
                command.Parameters.AddWithValue("@postId", postId);
                connection.Open();
                return (int)command.ExecuteScalar();

            }
        }

        private Post GetPostFromReader(SqlDataReader reader)
        {
            Post post = new Post();
            post.Id = (int)reader["Id"];
            post.Title = (string)reader["Title"];
            post.Content = (string)reader["Content"];
            post.Date = (DateTime)reader["Date"];
            return post;
        }
    }
}