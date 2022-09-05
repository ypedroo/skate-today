namespace shouldISkateToday.Domain.Models;

public class SkatePark
{
    public string? Name { get; set; }
    public double Rating { get; set; }
    public string? Reference { get; set; }
    public int UserRatingsTotal { get; set; }
}