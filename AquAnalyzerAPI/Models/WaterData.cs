using System;

public class WaterData
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double UsageVolume { get; set; }
    public double FlowRate { get; set; }
    public double ElectricityConsumption { get; set; }
    public double ProductId { get; set; }
    // freshwater, recycled, groundwater, seawater?
    public string SourceType { get; set; }
    public bool LeakDetected? { get; set; }
    public string Location { get; set; }
    public bool HasAbnormalities { get; set; }
    public bool UsesCleanEnergy { get; set; }

    public WaterData() { }

    public WaterData()
    {
        Timestamp = DateTime.Now;
        IsAnomalous = false;
    }
}
