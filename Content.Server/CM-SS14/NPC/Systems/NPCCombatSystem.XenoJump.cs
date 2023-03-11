using Content.Server.NPC.Components;
using Content.Shared.CombatMode;
using Content.Shared.Interaction;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;
using Content.Server.CMSS14.NPC.Components;
using Content.Shared.Throwing;

namespace Content.Server.NPC.Systems;

public sealed partial class NPCCombatSystem
{
    // [Dependency] private readonly RotateToFaceSystem _rotate = default!;
    [Dependency] private readonly ThrowingSystem _throwingSystem = default!;

    // // TODO: Don't predict for hitscan
    // private const float ShootSpeed = 20f;

    // /// <summary>
    // /// Cooldown on raycasting to check LOS.
    // /// </summary>
    // public const float UnoccludedCooldown = 0.2f;

    private void InitializeXenoJump()
    {
        SubscribeLocalEvent<NPCXenoJumpComponent, ComponentStartup>(OnXenoJumpStartup);
        SubscribeLocalEvent<NPCXenoJumpComponent, ComponentShutdown>(OnXenoJumpShutdown);
    }

    private void OnXenoJumpStartup(EntityUid uid, NPCXenoJumpComponent component, ComponentStartup args)
    {
        if (TryComp<SharedCombatModeComponent>(uid, out var combat))
        {
            combat.IsInCombatMode = true;
        }
        else
        {
            component.Status = CombatStatus.Unspecified;
        }
    }

    private void OnXenoJumpShutdown(EntityUid uid, NPCXenoJumpComponent component, ComponentShutdown args)
    {
        if (TryComp<SharedCombatModeComponent>(uid, out var combat))
        {
            combat.IsInCombatMode = false;
        }
    }

    private void UpdateXenoJump(float frameTime)
    {
        var bodyQuery = GetEntityQuery<PhysicsComponent>();
        var xformQuery = GetEntityQuery<TransformComponent>();
        var combatQuery = GetEntityQuery<SharedCombatModeComponent>();

        foreach (var (comp, xform) in EntityQuery<NPCXenoJumpComponent, TransformComponent>())
        {
            if (comp.HasJumped)
            {
                continue;
            }

            if (comp.Status == CombatStatus.Unspecified)
                continue;

            if (!xformQuery.TryGetComponent(comp.Target, out var targetXform) ||
                !bodyQuery.TryGetComponent(comp.Target, out var targetBody))
            {
                comp.Status = CombatStatus.TargetUnreachable;
                comp.ShootAccumulator = 0f;
                continue;
            }

            if (targetXform.MapID != xform.MapID)
            {
                comp.Status = CombatStatus.TargetUnreachable;
                comp.ShootAccumulator = 0f;
                continue;
            }

            if (combatQuery.TryGetComponent(comp.Owner, out var combatMode))
            {
                combatMode.IsInCombatMode = true;
            }

            comp.LOSAccumulator -= frameTime;

            var (worldPos, worldRot) = _transform.GetWorldPositionRotation(xform, xformQuery);
            var (targetPos, targetRot) = _transform.GetWorldPositionRotation(targetXform, xformQuery);

            Logger.Debug(" 1 ");

            // We'll work out the projected spot of the target and shoot there instead of where they are.
            var distance = (targetPos - worldPos).Length;
            var oldInLos = comp.TargetInLOS;

            // TODO: Should be doing these raycasts in parallel
            // Ideally we'd have 2 steps, 1. to go over the normal details for shooting and then 2. to handle beep / rotate / shoot
            if (comp.LOSAccumulator < 0f)
            {
                comp.LOSAccumulator += UnoccludedCooldown;
                comp.TargetInLOS = _interaction.InRangeUnobstructed(comp.Owner, comp.Target, distance + 0.1f);
            }

            Logger.Debug(" 2 ");

            if (!comp.TargetInLOS)
            {
                comp.ShootAccumulator = 0f;
                comp.Status = CombatStatus.TargetUnreachable;
                continue;
            }

            if (!oldInLos && comp.SoundTargetInLOS != null)
            {
                _audio.PlayPvs(comp.SoundTargetInLOS, comp.Owner);
            }

            comp.ShootAccumulator += frameTime;

            if (comp.ShootAccumulator < comp.ShootDelay)
            {
                continue;
            }

            Logger.Debug(" 3 ");

            var mapVelocity = targetBody.LinearVelocity;
            var targetSpot = targetPos + mapVelocity * distance / ShootSpeed;

            // If we have a max rotation speed then do that.
            var goalRotation = (targetSpot - worldPos).ToWorldAngle();
            var rotationSpeed = comp.RotationSpeed;

            if (!_rotate.TryRotateTo(comp.Owner, goalRotation, frameTime, comp.AccuracyThreshold, rotationSpeed?.Theta ?? double.MaxValue, xform))
            {
                continue;
            }

            Logger.Debug(" 4 ");

            if (!Enabled)
                continue;

            // throwDirection equals the target's position minus the shooter's position
            var throwDirection = targetSpot - worldPos;

            // throwStrength is the distance between the target and the shooter halved, then clamped between 0 and 2
            var throwStrength = Math.Clamp(throwDirection.Length / 2, 1f, 3);

            _throwingSystem.TryThrow(comp.Owner, throwDirection, throwStrength);

            Logger.Debug(" 5 ");

            comp.HasJumped = true;
        }
    }
}
