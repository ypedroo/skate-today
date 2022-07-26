namespace shouldISkateToday;

public class RefreshToken
{
    public string Token { get; set; }  = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
}