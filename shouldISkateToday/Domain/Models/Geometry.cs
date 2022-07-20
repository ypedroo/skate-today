namespace shouldISkateToday.Domain.Models;

public class Geometry
{
    public Geometry(Location location)
    {
        Location = location;
    }

    public Location Location { get; set; }
}