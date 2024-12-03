using System;

public class WaterMetrics
{
    public int Id { get; set; }
    public DateTime DateGeneratedOn { get; set; }
    public double LeakageRate { get; set; }
    public double WaterEfficiencyRatio { get; set; }
    public double TotalWaterConsumption { get; set; }
    public double TotalWaterSaved { get; set; }
    public double RecycledWaterUsage { get; set; }

    public WaterMetrics()
    {

    }
    public WaterMetrics()
    {
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        HasAbnormalities = false;
    }
}
