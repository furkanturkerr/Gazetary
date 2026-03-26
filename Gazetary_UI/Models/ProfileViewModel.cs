using BlogProject.Dtos;
using Entities.Concrate;

namespace BlogProject.Models;

public class ProfileViewModel
{
    public ProfileDto User { get; set; } = new();
    public List<Comment> UserComments { get; set; } = new();
    public List<BlogPost> MostReadPosts { get; set; } = new();
    public List<BlogPost> LatestPosts { get; set; } = new();
    public int TotalReadCount { get; set; }
}