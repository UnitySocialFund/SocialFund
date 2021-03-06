﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DataModel.BlogModel;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Services
{
    public class BlogService
    {
        private BlogRepository _db;
        private MongoCollection<Blog> collection;

        public BlogService()
        {
            _db = new BlogRepository();
            collection = _db.GetCollection();
        }

        public Guid CreateBlog(Blog blog, int? groupId)
        {
            collection.Save(blog);
            if (groupId != null)
            {
                new GroupService().SetBlog(blog.Id, (int)groupId);
            }

            return blog.Id;
        }

        public Blog GetBlog(Guid blogId)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            return blog;
        }

        public Blog GetBlog(int groupId)
        {
            var query = Query<Blog>.EQ(e => e.GroupId, groupId);
            var blog = collection.FindOne(query);
            return blog;
        }

        public void AddPost(Guid blogId, Post post)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            if (blog.Posts == null)
            {
                blog.Posts = new List<Post>();
            }
            blog.Posts.Add(new Post
            {
                Id = Guid.NewGuid(),
                Title = post.Title,
                Author = post.Author,
                Description = post.Description,
                Created = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0),
                Comments = new Collection<Comment>(),
                NotTakeATest = post.NotTakeATest,
                ApprovedList = new Collection<int>(),
                NotApprovedList = new Collection<int>()
            });
            collection.Save<Blog>(blog);
        }

        public void AddComment(Guid blogId, Guid postId, Comment comm)
        {
            var blog = collection.FindOne(Query<Blog>.EQ(e => e.Id, blogId));
            if (blog == null) return;
            
            var post = blog.Posts.SingleOrDefault(x => x.Id == postId);
            if (post == null) return;
            
            post.CommentCount++;
            post.Comments.Add(comm);
            collection.Save<Blog>(blog);
        }

        public void AddReplyComment(Guid blogId, Guid postId, Guid commentId, Comment comment)
        {
            var blog = collection.FindOne(Query<Blog>.EQ(e => e.Id, blogId));
            if (blog == null) return;
            
            var post = blog.Posts.SingleOrDefault(x => x.Id == postId);
            if (post == null) return;
                
            var commentFor = post.Comments.SingleOrDefault(x => x.Id == commentId);
            if (commentFor == null) return;
                
            post.CommentCount++;
            commentFor.Comments.Add(comment);
            collection.Save<Blog>(blog);
        }

        public Post GetPost(Guid blogId, Guid postId)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            return blog.Posts.Single(x => x.Id == postId);
        }

        public void InDonePost(Guid blogId, Guid postId)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            if (blog == null)
            {
                return;
            }
            var post = blog.Posts.SingleOrDefault(x => x.Id == postId);
            if (post == null)
            {
                return;
            }
            post.IsDone = true;
            collection.Save<Blog>(blog);
        }

        public void FromDonePost(Guid blogId, Guid postId)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            if (blog == null)
            {
                return;
            }
            var post = blog.Posts.SingleOrDefault(x => x.Id == postId);
            if (post == null)
            {
                return;
            }
            post.IsDone = false;
            collection.Save<Blog>(blog);
        }

        public void UpdatePost(Guid blogId, Post post)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            var tmpPost = blog.Posts.SingleOrDefault(x => x.Id == post.Id);
            if (tmpPost == null)
            {
                return;
            }
            tmpPost.Title = post.Title;
            tmpPost.Description = post.Description;
            tmpPost.Author = post.Author;

            tmpPost.NotTakeATest = post.NotTakeATest;
            tmpPost.ApprovedList = post.ApprovedList;
            tmpPost.NotApprovedList = post.NotApprovedList;

            tmpPost.Force = post.Force;

            tmpPost.Modified = post.Modified;

            collection.Save<Blog>(blog);
        }

        public void RemovePost(Guid blogId, Guid postId)
        {
            var query = Query<Blog>.EQ(e => e.Id, blogId);
            var blog = collection.FindOne(query);
            var post = blog.Posts.SingleOrDefault(x => x.Id == postId);
            if (post != null)
            {
                blog.Posts.Remove(post);
                collection.Save<Blog>(blog);
            }

        }

        public void ClearDB()
        {
            collection.RemoveAll();
        }
    }
}
