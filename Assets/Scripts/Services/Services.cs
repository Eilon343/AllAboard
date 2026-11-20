// Services.cs: where every script finds the services
public static class Services
{
    public static ISaveService Save { get; set; }
    public static ISettingsService Settings { get; set; }
    // encountership get economy stats from services.config.economy
    public static IConfigService Config { get; set; }
}
