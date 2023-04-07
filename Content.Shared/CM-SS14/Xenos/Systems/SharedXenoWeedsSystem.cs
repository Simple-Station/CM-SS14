using Robust.Shared.Physics.Events;
using Content.Shared.Popups;
using Robust.Shared.Timing;
using Content.Shared.CMSS14.Xenos;
using Content.Shared.Mobs.Systems;
using Content.Shared.Bed.Sleep;
using Content.Shared.Damage;

namespace Content.Shared.CMSS14.Xenos.Weeds;

public sealed class SharedXenoWeedsSystem : EntitySystem
{
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] protected readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly SharedAppearanceSystem _appearance = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly DamageableSystem _damageableSystem = default!;

    // Subscribe to the start collide and end collide events.
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<XenoWeedssComponent, StartCollideEvent>(OnStartCollide);
        SubscribeLocalEvent<XenoWeedssComponent, EndCollideEvent>(OnEndCollide);
    }

    // Every frame, run code on each entity in the weeds list.
    public override void Update(float frameTime)
    {
        base.Update(frameTime);
        foreach (var weeds in EntityManager.EntityQuery<XenoWeedssComponent>())
        {
            if (_timing.CurTime < weeds.NextHealTime)
                continue;

            weeds.NextHealTime += TimeSpan.FromSeconds(weeds.HealTime);

            foreach (var entity in weeds.PresentEntities)
            {
                if (_mobStateSystem.IsDead(entity))
                    continue;

                var damage = weeds.Damage;

                if (HasComp<SleepingComponent>(entity) || _mobStateSystem.IsCritical(entity))
                    damage *= weeds.RestMultiplier;

                _damageableSystem.TryChangeDamage(entity, damage, true, origin: weeds.Owner);
            }
        }
    }

    // When an entity starts colliding with the weeds, add it to the list of entities present on the weeds.
    private void OnStartCollide(EntityUid uid, XenoWeedssComponent component, ref StartCollideEvent args)
    {
        var target = args.OtherFixture.Body.Owner;
        if (!component.PresentEntities.Contains(target) && _entityManager.TryGetComponent<XenoComponent>(target, out var xenoComponent))
        {
            if (component.PresentEntities.Count > 3)
            {
                if (!_timing.InPrediction)
                {
                    _popup.PopupEntity(Loc.GetString("xeno-weeds-list-too-big"), target, target);
                }
                return;
            }

            // If predicting, don't play the sound.
            if (!_timing.InPrediction)
            {
                _popup.PopupEntity(Loc.GetString("xeno-weeds-add-to-list"), target, target);
            }
            component.PresentEntities.Add(target);
        }
    }

    // When an entity stops colliding with the weeds, remove it from the list of entities present on the weeds.
    private void OnEndCollide(EntityUid uid, XenoWeedssComponent component, ref EndCollideEvent args)
    {
        var target = args.OtherFixture.Body.Owner;
        if (component.PresentEntities.Contains(target))
        {
            component.PresentEntities.Remove(target);
        }
    }

}
