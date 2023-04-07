using Content.Shared.Damage;

namespace Content.Shared.CMSS14.Xenos.Weeds;

[RegisterComponent]
public sealed class XenoWeedssComponent : Component
{
    /// <summary>
    ///     List of entities present on the weeds.
    [ViewVariables]
    public List<EntityUid> PresentEntities = new List<EntityUid>();

    /// <summary>
    ///     Damage specifier for the weeds.
    /// </summary>
    [DataField("damage", required: true)]
    [ViewVariables(VVAccess.ReadWrite)]
    public DamageSpecifier Damage = default!;

    /// <summary>
    ///     Multiplier for damage when healing a sleeping entity.
    /// </summary>
    [DataField("restMultiplier")]
    public float RestMultiplier = 3f;

    /// <summary>
    ///     Time between each heal, with a datafield.
    /// </summary>
    [ViewVariables]
    [DataField("healTime")]
    public float HealTime = 1f;

    /// <summary>.
    ///     Time until the next heal
    /// </summary>
    [ViewVariables]
    public TimeSpan NextHealTime = TimeSpan.Zero;
}
