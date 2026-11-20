using System;

/// <summary>Provides the EconomyConfig assigned to Bootstrap.</summary>
public sealed class ConfigService : IConfigService
{
    public EconomyConfig Economy { get; }

    /// <summary>Require a real EconomyConfig asset before game screens load.</summary>
    public ConfigService(EconomyConfig economy)
    {
        Economy = economy ?? throw new ArgumentNullException(nameof(economy));
    }
}
