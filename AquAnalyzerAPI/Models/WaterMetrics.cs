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
    public WaterMetrics(int Id, DateTime DateGeneratedOn)
    {
        this.Id = Id;
        this.DateGeneratedOn = DateGeneratedOn;
    }

    public WaterMetrics(int Id, DateTime DateGeneratedOn, double LeakageRate, double WaterEfficiencyRatio, double TotalWaterConsumption, double TotalWaterSaved, double RecycledWaterUsage)
    {
        this.Id = Id;
        this.DateGeneratedOn = DateGeneratedOn;
        this.LeakageRate = LeakageRate;
        this.WaterEfficiencyRatio = WaterEfficiencyRatio;
        this.TotalWaterConsumption = TotalWaterConsumption;
        this.TotalWaterSaved = TotalWaterSaved;
        this.RecycledWaterUsage = RecycledWaterUsage;
    }

}
