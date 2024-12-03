using System;

public class WaterData
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double WaterVolume { get; set; }
    public string Source { get; set; }
    public string Location { get; set; }
    public string Notes { get; set; }
    public bool HasAnomalies { get; set; }
    public WaterData()
    {
        Timestamp = DateTime.Now;
        IsAnomalous = false;
    }
}
