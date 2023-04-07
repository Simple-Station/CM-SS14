using Robust.Client.GameObjects;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Content.Shared.SimpleStation14.Overlays;
using Robust.Shared.Prototypes;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Actions;

namespace Content.Client.SimpleStation14.Overlays;
public sealed class NightvisionSystem : EntitySystem
{
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IOverlayManager _overlayMan = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedActionsSystem _action = default!;

    private NightvisionOverlay _overlay = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<NightvisionComponent, ComponentStartup>(OnNightvisionStartup);
        SubscribeLocalEvent<NightvisionComponent, ComponentShutdown>(OnNightvisionShutdown);

        SubscribeLocalEvent<NightvisionComponent, PlayerAttachedEvent>(OnPlayerAttached);
        SubscribeLocalEvent<NightvisionComponent, PlayerDetachedEvent>(OnPlayerDetached);

        SubscribeLocalEvent<NightvisionComponent, NightvisionToggleActionEvent>(OnNightvisionToggle);

        _overlay = new();
    }


    private void OnNightvisionToggle(EntityUid uid, NightvisionComponent component, NightvisionToggleActionEvent args)
    {
        if (_player.LocalPlayer?.ControlledEntity == null)
        {
            Logger.Warning("Tried to toggle nightvision without a player!");
            return;
        }

        // var uid = _player.LocalPlayer.ControlledEntity.Value;
        // var component = _entityManager.GetComponent<NightvisionComponent>(uid);

        Logger.Warning("Toggled nightvision!");

        if (!component.Enabled)
        {
            component.Enabled = true;
            _overlayMan.AddOverlay(_overlay);
        }
        else
        {
            component.Enabled = false;
            _overlayMan.RemoveOverlay(_overlay);
        }
    }

    /// <summary>
    ///     Handles adding the toggle Nightvision action to a player, based on the <see cref="NightvisionComponent"/>.
    /// </summary>
    /// <param name="uid">The entity to add the action to.</param>
    /// <param name="component">The component to use for the action.</param>
    /// <param name="provider">The entity that provides the action. Defaults to the entity itself.</param>
    /// <param name="enabled">Whether or not the action is enabled immediately.</param>
    private void AddNightvisionAction(EntityUid uid, NightvisionComponent component, EntityUid? provider = null, bool enabled = false)
    {
        if (!_prototypeManager.TryIndex<InstantActionPrototype>("NightvisionToggleAction", out var toggleAction))
            return;

        var action = new InstantAction(toggleAction);

        action.DisplayName = component.ActionName;
        action.Description = component.ActionDescription;

        if (component.Icon != null)
        {
            action.Icon = component.Icon;

            if (component.IconOn != null)
                action.IconOn = component.IconOn;
        }
        else

        if (component.ToggleSound != null)
        {
            action.Sound = component.ToggleSound;
        }

        _action.AddAction(uid, action, provider ?? uid);

        if (enabled)
        {
            _action.SetToggled(action, true);
        }
    }

    private void AddNightvision(EntityUid uid, NightvisionComponent component, EntityUid? provider = null)
    {
        if (component.Enabled)
        {
            _overlayMan.AddOverlay(_overlay);
        }

        if (component.Toggle)
        {
            AddNightvisionAction(uid, component, uid, component.Enabled);
        }
    }

    private void RemoveOverlay(EntityUid uid, NightvisionComponent component)
    {
        _overlayMan.RemoveOverlay(_overlay);
    }


    private void OnNightvisionStartup(EntityUid uid, NightvisionComponent component, ComponentStartup args)
    {
        if (component.Granted && _player.LocalPlayer?.ControlledEntity == uid)
            AddNightvision(uid, component);
    }

    private void OnNightvisionShutdown(EntityUid uid, NightvisionComponent component, ComponentShutdown args)
    {
        if (component.Granted && _player.LocalPlayer?.ControlledEntity == uid)
            RemoveOverlay(uid, component);
    }

    private void OnPlayerAttached(EntityUid uid, NightvisionComponent component, PlayerAttachedEvent args)
    {
        if (component.Granted)
            AddNightvision(uid, component);
    }

    private void OnPlayerDetached(EntityUid uid, NightvisionComponent component, PlayerDetachedEvent args)
    {
        if (component.Granted)
            RemoveOverlay(uid, component);
    }
}
