using Content.Shared.Throwing;
using Content.Server.CMSS14.Xenos.Components;
using Content.Shared.Popups;
using Robust.Shared.Player;

namespace Content.Server.CMSS14.Xenos;

// Popup system dependency

public sealed class FacehuggerSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FacehuggerComponent, ThrowDoHitEvent>(OnHit);
    }

    private void OnHit(EntityUid uid, FacehuggerComponent component, ThrowDoHitEvent args)
    {
        _popup.PopupEntity(Loc.GetString("facehugger-hit"), uid, PopupType.MediumCaution);
    }
}
