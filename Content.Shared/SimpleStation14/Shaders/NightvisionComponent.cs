namespace Content.Shared.SimpleStation14.Overlays;

[RegisterComponent]
public sealed class NightvisionComponent : Component
{
    /// <summary>
    ///     The tint color to apply to the screen, in RGB. Defaults to green.
    /// </summary>
    [DataField("tint"), ViewVariables(VVAccess.ReadWrite)]
    public Vector3 Tint = new(0.0f, 1f, 0.1f);

    /// <summary>
    ///     The amount to boost the brightness of the screen.
    /// </summary>
    [DataField("strength"), ViewVariables(VVAccess.ReadWrite)]
    public float Strength = 0.7f;

    /// <summary>
    ///     The intensity of the noise effect.
    /// </summary>
    [DataField("noise"), ViewVariables(VVAccess.ReadWrite)]
    public float Noise = 0.5f;

    /// <summary>
    ///     Whether or not the nightvision is inherient to the entity.
    /// </summary>
    [DataField("inherint"), ViewVariables(VVAccess.ReadWrite)]
    public bool Inherint;

    /// <summary>
    ///     The entity that granted this nightvision, if valid
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? GranterUid;


    /// <summary>
    ///     Can the nightvision be toggled on and off via action?
    /// </summary>
    [DataField("toggle"), ViewVariables(VVAccess.ReadOnly)]
    public bool Toggle = false;

    /// <summary>
    ///     Is the nightvision currently enabled?
    /// </summary>
    [DataField("enabled"), ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled = false;


    [DataField("sprite")]
    public string rsi = "Objects/Weapons/Guns/Projectiles/Projectiles.rsi";

    [DataField("state")]
    public string state = "bolt";
}
