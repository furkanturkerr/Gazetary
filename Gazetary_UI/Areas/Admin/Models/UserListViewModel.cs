namespace BlogProject.Areas.Admin.Models;

public class UserListViewModel
{
    public List<UserViewModel> Admins { get; set; } = new();
    public List<UserViewModel> Users  { get; set; } = new();
}
 
public class UserViewModel
{
    public string Id             { get; set; }
    public string NameSurname    { get; set; }
    public string Email          { get; set; }
    public bool   EmailConfirmed { get; set; }
    public string Role           { get; set; }
}