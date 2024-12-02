public class VisualDesigner : User
{
    public List<WaterMetrics> GeneratedReports { get; set; }
    public VisualDesigner()
    {
    }

    public VisualDesigner(int id, string username) : base(id, username)
    {
    }

    public VisualDesigner(int id, string username, string password, string email, string role) : base(id, username, password, email, role)
    {
    }
}