namespace shouldISkateToday.Domain.Models;

public class Geometry
{
    public Geometry(Location location)
    {
        Location = location;
    }

    private Location Location { get; set; }
}