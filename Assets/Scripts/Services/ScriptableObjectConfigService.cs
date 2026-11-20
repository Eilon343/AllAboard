
public class ScriptableObjectConfigService : IConfigService
{
    public EconomyConfig Economy { get; }
    public ScriptableObjectConfigService(EconomyConfig economyConfig) => Economy = economyConfig;
}
