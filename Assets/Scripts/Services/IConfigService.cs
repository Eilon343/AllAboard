/// <summary>Exposes game configuration without making screens own the asset.</summary>
public interface IConfigService
{
    EconomyConfig Economy { get; }
}
