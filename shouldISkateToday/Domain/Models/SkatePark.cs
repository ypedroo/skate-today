namespace shouldISkateToday.Domain.Models;

public class SkatePark
{
    public Coordinates Coordinates { get; set; }
    public string Name { get; init; }
    public string Weather { get; init; }
}