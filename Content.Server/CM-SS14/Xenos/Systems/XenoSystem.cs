using Content.Shared.Tag;
using Content.Shared.Popups;
using Content.Shared.Actions;
using Content.Shared.Actions.ActionTypes;
using Content.Shared.Pulling;
using Content.Shared.Administration.Logs;
using Robust.Server.Player;
using Robust.Shared.Prototypes;
using Content.Shared.CMSS14.Xenos;
using Content.Server.Humanoid;
using Content.Server.Preferences.Managers;
using Content.Shared.Mobs.Systems;
using Content.Shared.Inventory;

namespace Content.Server.CMSS14.Xenos;

public class HologramSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] protected readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedPullingSystem _pulling = default!;
    [Dependency] private readonly ISharedAdminLogManager _adminLogger = default!;
    [Dependency] private readonly IPlayerManager _playerManager = null!;
    [Dependency] private readonly HumanoidAppearanceSystem _humanoidSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly TagSystem _tag = default!;
    [Dependency] private readonly IServerPreferencesManager _prefs = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly SharedActionsSystem _actionSystem = default!;

    public readonly Dictionary<Mind.Mind, EntityUid> ClonesWaitingForMind = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<XenoComponent, ComponentStartup>(Startup);
        SubscribeLocalEvent<XenoComponent, ComponentShutdown>(Shutdown);
        SubscribeLocalEvent<XenoComponent, XenoRestActionEvent>(OnXenoRestEvent);
    }

    private void Startup(EntityUid uid, XenoComponent component, ComponentStartup args)
    {
        if (_prototypeManager.TryIndex<InstantActionPrototype>(("XenoRestAction"), out var restAction))
        {
            _actionSystem.AddAction(uid, new InstantAction(restAction), null);
        }
    }

    private void Shutdown(EntityUid uid, XenoComponent component, ComponentShutdown args)
    {
        if (_prototypeManager.TryIndex<InstantActionPrototype>(("XenoRestAction"), out var restAction))
        {
            _actionSystem.RemoveAction(uid, new InstantAction(restAction));
        }
    }

    private void OnXenoRestEvent(EntityUid xeno, XenoComponent xenoComp, XenoRestActionEvent args)
    {
        RaiseLocalEvent(xeno, new SleepActionEvent());
    }
}
