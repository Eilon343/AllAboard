// Services.cs: where every script finds the services
public static class Services
{
    public static ISaveService Save { get; set; }
    public static ISettingsService Settings { get; set; }
    public static IConfigService Config { get; set; }
    public static ShipHealth PlayerHealth { get; set; }
}
