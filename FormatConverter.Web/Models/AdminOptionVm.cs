namespace FormatConverter.Web.Models;

public class AdminActionVm
{
    public AdminActionVm(string action, string title)
    {
        Action = action;
        Title = title;
    }

    public string Action { get; set; }
    public string Title { get; set; }
}
