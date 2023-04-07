using Content.Shared.Actions;
using Robust.Shared.Audio;
using Robust.Shared.Utility;

namespace Content.Shared.SimpleStation14.Overlays;


public sealed class NightvisionToggleActionEvent : InstantActionEvent { }


[RegisterComponent]
public sealed class NightvisionComponent : Component
{
    /// <summary>
    ///     The tint color to apply to the screen, in RGB. Defaults to green.
    /// </summary>
    [DataField("tint"), ViewVariables(VVAccess.ReadWrite)]
    public Vector3 Tint = new(0.5f, 0.5f, 0.5f);

    /// <summary>
    ///     The amount to boost the brightness of the screen.
    /// </summary>
    [DataField("strength"), ViewVariables(VVAccess.ReadWrite)]
    public float Strength = 5f;

    /// <summary>
    ///     The intensity of the noise effect.
    /// </summary>
    [DataField("noise"), ViewVariables(VVAccess.ReadWrite)]
    public float Noise = 0.0f;

    /// <summary>
    ///     Whether or not the nightvision applies to the owner.
    ///     Would be false for equipment that grants nightvision.
    /// </summary>
    /// <remarks>
    ///     USEINHAND EQUIPCLOTHING
    /// </remarks>
    [DataField("granted"), ViewVariables(VVAccess.ReadWrite)]
    public bool Granted = true;

    /// <summary>
    ///     The entity that granted this nightvision, if valid
    /// </summary>
    [ViewVariables(VVAccess.ReadOnly)]
    public EntityUid? GranterUid;

    /// <summary>
    ///     Can the nightvision be toggled on and off via action?
    ///     If true, grants an action. Action details can be defined in the YAML.
    /// </summary>
    [DataField("toggle"), ViewVariables(VVAccess.ReadOnly)]
    public bool Toggle = false;

    /// <summary>
    ///     Is the nightvision currently enabled?
    /// </summary>
    [DataField("enabled"), ViewVariables(VVAccess.ReadWrite)]
    public bool Enabled = true;


    // For customizing the action.

    /// <summary>
    ///     The name of the action.
    /// </summary>
    [DataField("actionName")]
    public string ActionName = "actions-nightvision-toggle-name";

    /// <summary>
    ///     The description of the action.
    /// </summary>
    [DataField("actionDescription")]
    public string ActionDescription = "actions-nightvision-toggle-description";

    /// <summary>
    ///     Icon state for the action.
    ///     If null, the entity's icon will be used.
    /// </summary>
    [DataField("icon")]
    public SpriteSpecifier? Icon;

    /// <summary>
    ///     State for when the night vision is toggled on.
    ///     If null, the regular state will be used.
    /// </summary>
    /// <remarks>
    ///     This *must* be used with <see cref="IconOn"/>, not by itself.
    /// </remarks>
    [DataField("iconOn")]
    public SpriteSpecifier? IconOn;

    /// <summary>
    ///     Sound to play when the nightvision is toggled.
    /// </summary>
    [DataField("toggleSound")]
    public SoundSpecifier? ToggleSound;
}
