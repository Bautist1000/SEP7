public class Abnormality
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public double AcceptableRangeMin { get; set; }
    public double AcceptableRangeMax { get; set; }
}
