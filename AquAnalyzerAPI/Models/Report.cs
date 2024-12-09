public class Report {
    public int Id {get; set;}
    public string Title {get; set;}
    public string Description {get; set;}
    public List<Visualisation> Visualisations = [];
    public int UserId {get; set;}
    public DateTime GeneratedDate {get; set;}
}