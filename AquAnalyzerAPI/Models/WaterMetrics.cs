using System;

public class WaterMetrics
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public double TotalConsumption { get; set; }
    public double WaterEfficiency { get; set; }
    public double Savings { get; set; }
    public bool HasAbnormalities { get; set; }
    public string Insights { get; set; }
    public WaterMetrics()
    {
        StartDate = DateTime.Now;
        EndDate = DateTime.Now;
        HasAbnormalities = false;
    }
}
